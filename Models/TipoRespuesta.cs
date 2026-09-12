namespace BlazorEncuesta.Models;

public class TipoRespuesta
{
    public int Clave { get; set; }
    public string Nombre { get; set; } = string.Empty;

    // Valores predefinidos
    public static readonly TipoRespuesta Checkbox = new() { Clave = 1, Nombre = "Checkbox" };
    public static readonly TipoRespuesta RadioButton = new() { Clave = 2, Nombre = "RadioButton" };
    public static readonly TipoRespuesta TextoLibre = new() { Clave = 3, Nombre = "TextoLibre" };

    public static List<TipoRespuesta> Todos => new() { Checkbox, RadioButton, TextoLibre };
}