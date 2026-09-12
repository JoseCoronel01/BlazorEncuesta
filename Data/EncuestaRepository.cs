using BlazorEncuesta.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEncuesta.Data;

public class EncuestaRepository
{
    private readonly AppDbContext _context;

    public EncuestaRepository(AppDbContext context)
    {
        _context = context;
    }

    // ObjectQueryPattern en acción
    public async Task<(List<Encuesta> Items, int Total)> GetByQueryAsync(EncuestaQuery query)
    {
        var filter = query.BuildFilter();
        var q = _context.Encuestas.AsNoTracking().Where(filter);

        if (query.IncluirPreguntas == true)
            q = q.Include(e => e.Preguntas);

        // Ordenamiento dinámico
        q = query.OrdenarPor?.ToLower() switch
        {
            "nombre" => query.OrdenDescendente ? q.OrderByDescending(e => e.Nombre) : q.OrderBy(e => e.Nombre),
            "vencimiento" => query.OrdenDescendente ? q.OrderByDescending(e => e.FechaVencimiento) : q.OrderBy(e => e.FechaVencimiento),
            _ => query.OrdenDescendente ? q.OrderByDescending(e => e.FechaEmision) : q.OrderBy(e => e.FechaEmision)
        };

        var total = await q.CountAsync();
        var items = await q
            .Skip((query.Pagina - 1) * query.TamanoPagina)
            .Take(query.TamanoPagina)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Encuesta?> GetByIdAsync(int clave, bool includePreguntas = false)
    {
        var query = _context.Encuestas.AsNoTracking();
        if (includePreguntas)
            query = query.Include(e => e.Preguntas);
        return await query.FirstOrDefaultAsync(e => e.Clave == clave);
    }

    public async Task<List<Pregunta>> GetPreguntasByQueryAsync(PreguntaQuery query)
    {
        var filter = query.BuildFilter();
        return await _context.Preguntas
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
    }

    public async Task<Encuesta> CreateAsync(Encuesta encuesta)
    {
        _context.Encuestas.Add(encuesta);
        await _context.SaveChangesAsync();
        return encuesta;
    }

    public async Task UpdateAsync(Encuesta encuesta)
    {
        _context.Encuestas.Update(encuesta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int clave)
    {
        var encuesta = await _context.Encuestas.FindAsync(clave);
        if (encuesta != null)
        {
            _context.Encuestas.Remove(encuesta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Pregunta> AddPreguntaAsync(Pregunta pregunta)
    {
        _context.Preguntas.Add(pregunta);
        await _context.SaveChangesAsync();
        return pregunta;
    }

    public async Task SaveRespuestaAsync(int preguntaClave, string respuesta)
    {
        var pregunta = await _context.Preguntas.FindAsync(preguntaClave);
        if (pregunta != null)
        {
            pregunta.Respuestas = string.IsNullOrEmpty(pregunta.Respuestas)
                ? respuesta
                : pregunta.Respuestas + ";" + respuesta;
            await _context.SaveChangesAsync();
        }
    }

    // Dashboard: estadísticas agregadas usando ObjectQueryPattern
    public async Task<DashboardStats> GetDashboardStatsAsync(EncuestaQuery query)
    {
        var filter = query.BuildFilter();
        var encuestas = await _context.Encuestas
            .AsNoTracking()
            .Where(filter)
            .Include(e => e.Preguntas)
            .ToListAsync();

        var stats = new DashboardStats
        {
            TotalEncuestas = encuestas.Count,
            EncuestasActivas = encuestas.Count(e => e.EstaActiva && !e.EstaVencida),
            EncuestasVencidas = encuestas.Count(e => e.EstaVencida),
            EncuestasPorVencer = encuestas.Count(e => !e.EstaVencida && e.FechaVencimiento <= DateTime.Now.AddDays(3)),
            TotalPreguntas = encuestas.Sum(e => e.Preguntas.Count),
            TotalRespuestas = encuestas.Sum(e => e.Preguntas.Sum(p => p.TotalRespuestas)),
            Encuestas = encuestas
        };

        return stats;
    }
}

public class DashboardStats
{
    public int TotalEncuestas { get; set; }
    public int EncuestasActivas { get; set; }
    public int EncuestasVencidas { get; set; }
    public int EncuestasPorVencer { get; set; }
    public int TotalPreguntas { get; set; }
    public int TotalRespuestas { get; set; }
    public List<Encuesta> Encuestas { get; set; } = new();
}