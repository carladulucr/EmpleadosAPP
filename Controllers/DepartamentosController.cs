using Microsoft.AspNetCore.Http;
using EmpleadosAPI.Data;
using EmpleadosAPI.DTOs;
using EmpleadosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosAPI.Controllers;

[ApiController]
[Route("api/departamentos")]
public class DepartamentosController : ControllerBase
{
    private readonly EmpleadosDbContext _context;

    public DepartamentosController(EmpleadosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departamentos = await _context.Departamentos
            .Select(d => new DepartamentoDTO
            {
                DepartamentoID = d.DepartamentoId,
                Nombre = d.Nombre
            })
            .ToListAsync();

        return Ok(new { data = departamentos });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await _context.Departamentos.FindAsync(id);
        if (d is null) return NotFound();
        return Ok(new DepartamentoDTO { DepartamentoID = d.DepartamentoId, Nombre = d.Nombre });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearDepartamentoDTO dto)
    {
        var departamento = new Departamento { Nombre = dto.Nombre };
        _context.Departamentos.Add(departamento);
        await _context.SaveChangesAsync();
        return Created($"/api/departamentos/{departamento.DepartamentoId}", departamento.DepartamentoId);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CrearDepartamentoDTO dto)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento is null) return NotFound();
        departamento.Nombre = dto.Nombre;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento is null) return NotFound();
        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}