using EmpleadoMVC.ViewModels.Departamentos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EmpleadoMVC.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartamentosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync("api/departamentos");
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultado = JsonSerializer.Deserialize<ApiListResponse<DepartamentoItemViewModel>>(json, options);

            var viewModel = new DepartamentoListViewModel
            {
                Departamentos = resultado?.Data ?? new List<DepartamentoItemViewModel>()
            };

            return View(viewModel);
        }

        public IActionResult Create() => View(new DepartamentoFormViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(DepartamentoFormViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                var client = _httpClientFactory.CreateClient("EmpleadosAPI");
                var payload = new { viewModel.Nombre };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PostAsync("api/departamentos", content);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                viewModel.Exception = ex;
                return View(viewModel);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"api/departamentos/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dept = JsonSerializer.Deserialize<DepartamentoItemViewModel>(json, options);

            return View(new DepartamentoFormViewModel { DepartamentoID = dept!.DepartamentoID, Nombre = dept.Nombre });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepartamentoFormViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                var client = _httpClientFactory.CreateClient("EmpleadosAPI");
                var payload = new { viewModel.Nombre };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PatchAsync($"api/departamentos/{id}", content);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                viewModel.Exception = ex;
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            await client.DeleteAsync($"api/departamentos/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
