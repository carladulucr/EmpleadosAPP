using System.ComponentModel.DataAnnotations;

namespace EmpleadoMVC.ViewModels.Empleados
{
    public class EmpleadoFormViewModel
    {
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un departamento.")]
        [Display(Name = "Departamento")]
        public int DepartamentoID { get; set; }

        [Required(ErrorMessage = "Seleccione un puesto.")]
        [Display(Name = "Puesto")]
        public int PuestoID { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor que cero.")]
        [Display(Name = "Salario")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [Display(Name = "Fecha de nacimiento")]
        public string FechaNacimiento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de contratación es obligatoria.")]
        [Display(Name = "Fecha de contratación")]
        public string FechaContratacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        // Para llenar los dropdowns en el formulario
        public List<SelectOption> Departamentos { get; set; } = new();
        public List<SelectOption> Puestos { get; set; } = new();

        // Para mostrar errores del servidor 
        public Exception? Exception { get; set; }
    }

    public class SelectOption
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
