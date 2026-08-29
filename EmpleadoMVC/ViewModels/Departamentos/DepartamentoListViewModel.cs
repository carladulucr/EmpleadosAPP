
using EmpleadoMVC.ViewModels;
namespace EmpleadoMVC.ViewModels.Departamentos
{
    public class DepartamentoListViewModel
    {
        public IEnumerable<DepartamentoItemViewModel> Departamentos { get; set; } = new List<DepartamentoItemViewModel>();
    }

    public class DepartamentoItemViewModel
    {
        public int DepartamentoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class DepartamentoFormViewModel
    {
        public int DepartamentoID { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        public Exception? Exception { get; set; }
    }
}
