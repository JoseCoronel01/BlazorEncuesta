using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEncuesta.Models;

public class Pregunta
{
    [Key]
    public int Clave { get; set; }

    [Required]
    [StringLength(500)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Encuesta")]
    public int EncuestaClave { get; set; }
    public Encuesta Encuesta { get; set; } = null!;

    [Required]
    [Display(Name = "Tipo de Respuesta")]
    public int TipoRespuestaClave { get; set; }

    [NotMapped]
    public string TipoRespuestaNombre => TipoRespuesta.Todos
        .FirstOrDefault(t => t.Clave == TipoRespuestaClave)?.Nombre ?? "Desconocido";

    // Opciones separadas por | para checkbox/radio
    public string? Opciones { get; set; }

    // Respuestas recibidas (almacenadas como JSON o separadas por ;)
    public string? Respuestas { get; set; }

    [NotMapped]
    public List<string> OpcionesList => Opciones?.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();

    [NotMapped]
    public List<string> RespuestasList => Respuestas?.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new();

    [NotMapped]
    public int TotalRespuestas => RespuestasList.Count;
}