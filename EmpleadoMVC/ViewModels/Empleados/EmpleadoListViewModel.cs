using EmpleadoMVC.ViewModels;

namespace EmpleadoMVC.ViewModels.Empleados
{
    public class EmpleadoListViewModel
    {
        public IEnumerable<EmpleadoViewModel> Empleados { get; set; } = new List<EmpleadoViewModel>();
        public PaginacionInfo Pagination { get; set; } = new();
        public string? Search { get; set; }
        public string? Apellido { get; set; }
        public string? FechaContratacion { get; set; }
    }
}
