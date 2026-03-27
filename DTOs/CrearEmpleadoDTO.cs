using System.ComponentModel.DataAnnotations;

namespace EmpleadosAPI.DTOs;

public class CrearEmpleadoDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    public int DepartamentoID { get; set; }

    [Required]
    public int PuestoID { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor que cero.")]
    public decimal Salario { get; set; }

    [Required]
    public DateOnly FechaNacimiento { get; set; }

    [Required]
    public DateOnly FechaContratacion { get; set; }

    [Required]
    [MaxLength(250)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string CorreoElectronico { get; set; } = string.Empty;
}