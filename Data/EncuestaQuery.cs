using BlazorEncuesta.Models;
using System.Linq.Expressions;

namespace BlazorEncuesta.Data;

public class EncuestaQuery
{
    public int? Clave { get; set; }
    public string? Nombre { get; set; }
    public DateTime? FechaEmisionDesde { get; set; }
    public DateTime? FechaEmisionHasta { get; set; }
    public DateTime? FechaVencimientoDesde { get; set; }
    public DateTime? FechaVencimientoHasta { get; set; }
    public bool? SoloActivas { get; set; }
    public bool? IncluirPreguntas { get; set; }
    public string? OrdenarPor { get; set; }
    public bool OrdenDescendente { get; set; }

    // Paginación
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;

    // Construye la expresión de filtro
    public Expression<Func<Encuesta, bool>> BuildFilter()
    {
        return e =>
            (!Clave.HasValue || e.Clave == Clave.Value) &&
            (string.IsNullOrEmpty(Nombre) || e.Nombre.Contains(Nombre)) &&
            (!FechaEmisionDesde.HasValue || e.FechaEmision >= FechaEmisionDesde.Value) &&
            (!FechaEmisionHasta.HasValue || e.FechaEmision <= FechaEmisionHasta.Value) &&
            (!FechaVencimientoDesde.HasValue || e.FechaVencimiento >= FechaVencimientoDesde.Value) &&
            (!FechaVencimientoHasta.HasValue || e.FechaVencimiento <= FechaVencimientoHasta.Value) &&
            (!SoloActivas.HasValue || !SoloActivas.Value ||
             (DateTime.Now >= e.FechaEmision && DateTime.Now <= e.FechaVencimiento.AddDays(7)));
    }
}