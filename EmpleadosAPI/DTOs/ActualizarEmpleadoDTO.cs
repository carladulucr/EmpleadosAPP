namespace EmpleadosAPI.DTOs;

public class ActualizarEmpleadoDTO
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public int? DepartamentoID { get; set; }
    public int? PuestoID { get; set; }
    public decimal? Salario { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public DateOnly? FechaContratacion { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
}