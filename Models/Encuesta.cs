using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEncuesta.Models;

public class Encuesta
{
    [Key]
    public int Clave { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Fecha de Emisión")]
    public DateTime FechaEmision { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Fecha de Vencimiento")]
    public DateTime FechaVencimiento { get; set; } = DateTime.Now.AddDays(7);

    // Navegación
    public List<Pregunta> Preguntas { get; set; } = new();

    [NotMapped]
    public bool EstaActiva => DateTime.Now >= FechaEmision && DateTime.Now <= FechaVencimiento.AddDays(7);

    [NotMapped]
    public bool EstaVencida => DateTime.Now > FechaVencimiento;

    [NotMapped]
    public int DiasRestantes => (FechaVencimiento.AddDays(7) - DateTime.Now).Days;
}