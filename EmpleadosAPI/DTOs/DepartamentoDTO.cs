using System.ComponentModel.DataAnnotations;

namespace EmpleadosAPI.DTOs;

public class DepartamentoDTO
{
    public int DepartamentoID { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class CrearDepartamentoDTO
{
    [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
}