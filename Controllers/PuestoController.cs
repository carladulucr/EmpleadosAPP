using EmpleadosAPI.Data;
using EmpleadosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosAPI.Controllers;

[ApiController]
[Route("api/puestos")]
public class PuestosController : ControllerBase
{
    private readonly EmpleadosDbContext _context;

    public PuestosController(EmpleadosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var puestos = await _context.Puestos
            .Select(p => new { p.PuestoId, p.Nombre })
            .ToListAsync();

        return Ok(new { data = puestos });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var puesto = await _context.Puestos.FindAsync(id);
        if (puesto is null) return NotFound();
        return Ok(new { puesto.PuestoId, puesto.Nombre });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearPuestoDTO dto)
    {
        var puesto = new Puesto { Nombre = dto.Nombre };
        _context.Puestos.Add(puesto);
        await _context.SaveChangesAsync();
        return Created($"/api/puestos/{puesto.PuestoId}", puesto.PuestoId);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CrearPuestoDTO dto)
    {
        var puesto = await _context.Puestos.FindAsync(id);
        if (puesto is null) return NotFound();
        puesto.Nombre = dto.Nombre;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var puesto = await _context.Puestos.FindAsync(id);
        if (puesto is null) return NotFound();
        _context.Puestos.Remove(puesto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class CrearPuestoDTO
{
    [System.ComponentModel.DataAnnotations.Required]
    public string Nombre { get; set; } = string.Empty;
}