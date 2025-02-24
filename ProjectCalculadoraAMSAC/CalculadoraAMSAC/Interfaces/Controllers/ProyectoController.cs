using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Authorize]

[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProyectoController(IProyectoQueryService queryService, IProyectoCommandService commandService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProyectos()
    {
        var query = new GetAllProyectosQuery();
        var proyectos = await queryService.Handle(query);
        return Ok(proyectos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProyectoById(int id)
    {
        var query = new GetProyectoByIdQuery(id);
        var proyecto = await queryService.Handle(query);

        if (proyecto == null) return NotFound("Project not found.");

        return Ok(proyecto);
    }

    [HttpPost("createProyecto")]
    public async Task<IActionResult> CreateProyecto([FromBody] CrearProyectoCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        // ✅ Crear el proyecto en la base de datos
        var id = await commandService.Handle(command);

        // ✅ Obtener el proyecto recién creado
        var proyecto = await queryService.Handle(new GetProyectoByIdQuery(id));
        if (proyecto == null) return NotFound("Project not found.");

        return CreatedAtAction(nameof(GetProyectoById), new { id }, proyecto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProyecto(int id, [FromBody] ActualizarProyectoCommand command)
    {
        if (command == null || id != command.ProyectoId) return BadRequest("Invalid input.");

        var success = await commandService.Handle(command);
        if (!success) return NotFound("Project not found.");

        return NoContent();
    }
}