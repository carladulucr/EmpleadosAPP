using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmpleadosAPI.Data;
using EmpleadosAPI.DTOs;
using EmpleadosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosAPI.Controllers;

[ApiController]
[Route("api/empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly EmpleadosDbContext _context;
    private readonly IWebHostEnvironment _env;

    public EmpleadosController(EmpleadosDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // GET api/empleados  (con paginación y filtros)
    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        string? nombre = null,
        string? apellido = null,
        DateOnly? fechaContratacion = null)
    {
        var query = _context.Empleados
            .Include(e => e.Departamento)
            .Include(e => e.Puesto)
            .Include(e => e.Estado)
            .AsQueryable();

        // Aplicar filtros antes de paginar 
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(e => e.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(apellido))
            query = query.Where(e => e.Apellido.Contains(apellido));

        if (fechaContratacion.HasValue)
            query = query.Where(e => e.FechaContratacion == fechaContratacion.Value);

        // Total se saca ANTES de paginar 
        var total = await query.CountAsync();

        var empleados = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EmpleadoDTO
            {
                EmpleadoID = e.EmpleadoId,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Departamento = e.Departamento.Nombre,
                Puesto = e.Puesto.Nombre,
                Salario = e.Salario,
                FechaNacimiento = e.FechaNacimiento,
                FechaContratacion = e.FechaContratacion,
                Direccion = e.Direccion,
                Telefono = e.Telefono,
                CorreoElectronico = e.CorreoElectronico,
                Estado = e.Estado.Nombre,
                FotografiaRuta = e.FotografiaRuta
            })
            .ToListAsync();

        var response = new PaginacionDTO<EmpleadoDTO>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Pages = (int)Math.Ceiling((double)total / pageSize),
            Data = empleados
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var empleado = await _context.Empleados
            .Include(e => e.Departamento)
            .Include(e => e.Puesto)
            .Include(e => e.Estado)
            .FirstOrDefaultAsync(e => e.EmpleadoId == id);

        if (empleado is null)
            return NotFound();

        var dto = new EmpleadoDTO
        {
            EmpleadoID = empleado.EmpleadoId,
            Nombre = empleado.Nombre,
            Apellido = empleado.Apellido,
            Departamento = empleado.Departamento.Nombre,
            Puesto = empleado.Puesto.Nombre,
            Salario = empleado.Salario,
            FechaNacimiento = empleado.FechaNacimiento,
            FechaContratacion = empleado.FechaContratacion,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            CorreoElectronico = empleado.CorreoElectronico,
            Estado = empleado.Estado.Nombre,
            FotografiaRuta = empleado.FotografiaRuta
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearEmpleadoDTO dto)
    {
        var empleado = new Empleado
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            DepartamentoId = dto.DepartamentoID,
            PuestoId = dto.PuestoID,
            Salario = dto.Salario,
            FechaNacimiento = dto.FechaNacimiento,
            FechaContratacion = dto.FechaContratacion,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            CorreoElectronico = dto.CorreoElectronico,
            EstadoId = 1   // Activo por defecto
        };

        try
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return Created($"/api/empleados/{empleado.EmpleadoId}", empleado.EmpleadoId);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarEmpleadoDTO dto)
    {
        var empleado = await _context.Empleados.FindAsync(id);

        if (empleado is null)
            return NotFound();

        if (dto.Nombre != null) empleado.Nombre = dto.Nombre;
        if (dto.Apellido != null) empleado.Apellido = dto.Apellido;
        if (dto.DepartamentoID.HasValue) empleado.DepartamentoId = dto.DepartamentoID.Value;
        if (dto.PuestoID.HasValue) empleado.PuestoId = dto.PuestoID.Value;
        if (dto.Salario.HasValue) empleado.Salario = dto.Salario.Value;
        if (dto.FechaNacimiento.HasValue) empleado.FechaNacimiento = dto.FechaNacimiento.Value;
        if (dto.FechaContratacion.HasValue) empleado.FechaContratacion = dto.FechaContratacion.Value;
        if (dto.Direccion != null) empleado.Direccion = dto.Direccion;
        if (dto.Telefono != null) empleado.Telefono = dto.Telefono;
        if (dto.CorreoElectronico != null) empleado.CorreoElectronico = dto.CorreoElectronico;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        empleado.EstadoId = 2; // Inactivo
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/reactivar")]
    public async Task<IActionResult> Reactivar(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        empleado.EstadoId = 1; // Activo
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/despedir")]
    public async Task<IActionResult> Despedir(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        empleado.EstadoId = 3; // Despedido
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/fotografia")]
    public async Task<IActionResult> SubirFotografia(int id, IFormFile foto)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        if (foto == null || foto.Length == 0)
            return BadRequest(new { error = "El archivo está vacío." });

        long maxSize = 10 * 1024 * 1024; // 10 MB
        if (foto.Length > maxSize)
            return BadRequest(new { error = "El archivo supera los 10 MB." });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(foto.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { error = "Solo se permiten imágenes JPG y PNG." });

        var uploadFolder = Path.Combine(_env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(uploadFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await foto.CopyToAsync(stream);
        }

        // Guardar la ruta relativa en la base de datos
        empleado.FotografiaRuta = $"Uploads/{uniqueFileName}";
        await _context.SaveChangesAsync();

        return Ok(new { ruta = empleado.FotografiaRuta });
    }

    // GET api/empleados/{id}/fotografia  (renderizar imagen)
    [HttpGet("{id}/fotografia")]
    public async Task<IActionResult> VerFotografia(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null || string.IsNullOrEmpty(empleado.FotografiaRuta))
            return NotFound();

        var filePath = Path.Combine(_env.ContentRootPath, empleado.FotografiaRuta);
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(stream, "image/jpeg"); // Content-Type 
    }
}