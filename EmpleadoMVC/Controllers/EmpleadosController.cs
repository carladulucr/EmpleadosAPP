using EmpleadoMVC.ViewModels;
using EmpleadoMVC.ViewModels.Empleados;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EmpleadoMVC.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBase = "api/empleados";

        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Empleados  (listado con paginación y filtros)
        public async Task<IActionResult> Index(
            int page = 1, int pageSize = 10,
            string? search = null, string? apellido = null,
            string? fechaContratacion = null)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var url = $"{_apiBase}?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search)) url += $"&nombre={search}";
            if (!string.IsNullOrWhiteSpace(apellido)) url += $"&apellido={apellido}";
            if (!string.IsNullOrWhiteSpace(fechaContratacion)) url += $"&fechaContratacion={fechaContratacion}";

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultado = JsonSerializer.Deserialize<ApiPaginadoResponse<EmpleadoViewModel>>(json, options);

            var viewModel = new EmpleadoListViewModel
            {
                Empleados = resultado?.Data ?? new List<EmpleadoViewModel>(),
                Pagination = new PaginacionInfo
                {
                    Page = resultado?.Page ?? 1,
                    PageSize = resultado?.PageSize ?? pageSize,
                    Pages = resultado?.Pages ?? 1,
                    Total = resultado?.Total ?? 0
                },
                Search = search,
                Apellido = apellido,
                FechaContratacion = fechaContratacion
            };

            return View(viewModel);
        }

        // GET: /Empleados/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"{_apiBase}/{id}");

            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var empleado = JsonSerializer.Deserialize<EmpleadoViewModel>(json, options);

            return View(empleado);
        }

        // GET: /Empleados/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EmpleadoFormViewModel();
            await CargarOpcionesFormulario(viewModel);
            return View(viewModel);
        }

        // POST: /Empleados/Create
        [HttpPost]
        public async Task<IActionResult> Create(EmpleadoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarOpcionesFormulario(viewModel);
                return View(viewModel);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("EmpleadosAPI");
                var payload = new
                {
                    viewModel.Nombre,
                    viewModel.Apellido,
                    viewModel.DepartamentoID,
                    viewModel.PuestoID,
                    viewModel.Salario,
                    viewModel.FechaNacimiento,
                    viewModel.FechaContratacion,
                    viewModel.Direccion,
                    viewModel.Telefono,
                    viewModel.CorreoElectronico
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_apiBase, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al crear el empleado en la API.");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                viewModel.Exception = ex;
                await CargarOpcionesFormulario(viewModel);
                return View(viewModel);
            }
        }

        // GET: /Empleados/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"{_apiBase}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var empleado = JsonSerializer.Deserialize<EmpleadoViewModel>(json, options);

            var viewModel = new EmpleadoFormViewModel
            {
                EmpleadoID = empleado!.EmpleadoID,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Salario = empleado.Salario,
                FechaNacimiento = empleado.FechaNacimiento,
                FechaContratacion = empleado.FechaContratacion,
                Direccion = empleado.Direccion,
                Telefono = empleado.Telefono,
                CorreoElectronico = empleado.CorreoElectronico
            };

            await CargarOpcionesFormulario(viewModel);
            return View(viewModel);
        }

        // POST: /Empleados/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmpleadoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarOpcionesFormulario(viewModel);
                return View(viewModel);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("EmpleadosAPI");
                var payload = new
                {
                    viewModel.Nombre,
                    viewModel.Apellido,
                    viewModel.DepartamentoID,
                    viewModel.PuestoID,
                    viewModel.Salario,
                    viewModel.FechaNacimiento,
                    viewModel.FechaContratacion,
                    viewModel.Direccion,
                    viewModel.Telefono,
                    viewModel.CorreoElectronico
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync($"{_apiBase}/{id}", content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al actualizar el empleado.");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                viewModel.Exception = ex;
                await CargarOpcionesFormulario(viewModel);
                return View(viewModel);
            }
        }

        // GET: /Empleados/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"{_apiBase}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var empleado = JsonSerializer.Deserialize<EmpleadoViewModel>(json, options);

            return View(empleado);
        }

        // POST: /Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.DeleteAsync($"{_apiBase}/{id}");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Empleados/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            await client.PatchAsync($"{_apiBase}/{id}/desactivar", null);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Empleados/Reactivar/5
        [HttpPost]
        public async Task<IActionResult> Reactivar(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            await client.PatchAsync($"{_apiBase}/{id}/reactivar", null);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Empleados/Despedir/5
        [HttpPost]
        public async Task<IActionResult> Despedir(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            await client.PatchAsync($"{_apiBase}/{id}/despedir", null);
            return RedirectToAction(nameof(Index));
        }


        // GET: /Empleados/UploadPhoto/5
        public async Task<IActionResult> UploadPhoto(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"{_apiBase}/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var empleado = JsonSerializer.Deserialize<EmpleadoViewModel>(json, options);

            ViewBag.EmpleadoNombre = $"{empleado!.Nombre} {empleado.Apellido}";
            ViewBag.EmpleadoID = id;
            return View();
        }

        // POST: /Empleados/UploadPhoto/5
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
            {
                ModelState.AddModelError("foto", "Selecciona una imagen.");
                ViewBag.EmpleadoID = id;
                return View();
            }

            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            using var form = new MultipartFormDataContent();
            using var stream = foto.OpenReadStream();
            form.Add(new StreamContent(stream), "foto", foto.FileName);

            var response = await client.PostAsync($"{_apiBase}/{id}/fotografia", form);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("foto", "Error al subir la fotografía.");
                ViewBag.EmpleadoID = id;
                return View();
            }

            return RedirectToAction(nameof(Details), new { id });
        }



        public async Task<IActionResult> VerFotografia(int id)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var response = await client.GetAsync($"api/empleados/{id}/fotografia");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";

            return File(imageBytes, contentType);
        }


        // Método privado para cargar los dropdowns 
        private async Task CargarOpcionesFormulario(EmpleadoFormViewModel viewModel)
        {
            var client = _httpClientFactory.CreateClient("EmpleadosAPI");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var deptResponse = await client.GetAsync("api/departamentos");
            var deptJson = await deptResponse.Content.ReadAsStringAsync();
            var deptResult = JsonSerializer.Deserialize<ApiListResponse<DeptOption>>(deptJson, options);
            viewModel.Departamentos = deptResult?.Data.Select(d => new SelectOption { Id = d.DepartamentoID, Nombre = d.Nombre }).ToList() ?? new();

            var puestoResponse = await client.GetAsync("api/puestos");
            var puestoJson = await puestoResponse.Content.ReadAsStringAsync();
            var puestoResult = JsonSerializer.Deserialize<ApiListResponse<PuestoOption>>(puestoJson, options);
            viewModel.Puestos = puestoResult?.Data.Select(p => new SelectOption { Id = p.PuestoID, Nombre = p.Nombre }).ToList() ?? new();
        }
    }



    // Clases auxiliares para deserializar respuestas de la API
    public class ApiPaginadoResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class ApiListResponse<T>
    {
        public List<T> Data { get; set; } = new();
    }

    public class DeptOption
    {
        public int DepartamentoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class PuestoOption
    {
        public int PuestoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }


}
