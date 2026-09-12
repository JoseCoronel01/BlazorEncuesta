using BlazorEncuesta.Data;
using BlazorEncuesta.Models;

namespace BlazorEncuesta.Services;

public class EncuestaService
{
    private readonly EncuestaRepository _repository;

    public EncuestaService(EncuestaRepository repository)
    {
        _repository = repository;
    }

    // Delegación al repositorio con ObjectQueryPattern
    public Task<(List<Encuesta> Items, int Total)> GetEncuestasAsync(EncuestaQuery query)
        => _repository.GetByQueryAsync(query);

    public Task<Encuesta?> GetEncuestaAsync(int clave, bool includePreguntas = false)
        => _repository.GetByIdAsync(clave, includePreguntas);

    public Task<Encuesta> CrearEncuestaAsync(Encuesta encuesta)
        => _repository.CreateAsync(encuesta);

    public Task ActualizarEncuestaAsync(Encuesta encuesta)
        => _repository.UpdateAsync(encuesta);

    public Task EliminarEncuestaAsync(int clave)
        => _repository.DeleteAsync(clave);

    public Task<List<Pregunta>> GetPreguntasAsync(PreguntaQuery query)
        => _repository.GetPreguntasByQueryAsync(query);

    public Task<Pregunta> AgregarPreguntaAsync(Pregunta pregunta)
        => _repository.AddPreguntaAsync(pregunta);

    public Task GuardarRespuestaAsync(int preguntaClave, string respuesta)
        => _repository.SaveRespuestaAsync(preguntaClave, respuesta);

    public Task<DashboardStats> GetDashboardStatsAsync(EncuestaQuery query)
        => _repository.GetDashboardStatsAsync(query);
}