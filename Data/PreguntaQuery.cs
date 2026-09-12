using BlazorEncuesta.Models;
using System.Linq.Expressions;

namespace BlazorEncuesta.Data;

public class PreguntaQuery
{
    public int? Clave { get; set; }
    public int? EncuestaClave { get; set; }
    public string? Nombre { get; set; }
    public int? TipoRespuestaClave { get; set; }

    public Expression<Func<Pregunta, bool>> BuildFilter()
    {
        return p =>
            (!Clave.HasValue || p.Clave == Clave.Value) &&
            (!EncuestaClave.HasValue || p.EncuestaClave == EncuestaClave.Value) &&
            (string.IsNullOrEmpty(Nombre) || p.Nombre.Contains(Nombre)) &&
            (!TipoRespuestaClave.HasValue || p.TipoRespuestaClave == TipoRespuestaClave.Value);
    }
}