namespace EmpleadoMVC.ViewModels.Empleados
{
    public class EmpleadoViewModel
    {
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public string FechaNacimiento { get; set; } = string.Empty;
        public string FechaContratacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? FotografiaRuta { get; set; }
    }
}
