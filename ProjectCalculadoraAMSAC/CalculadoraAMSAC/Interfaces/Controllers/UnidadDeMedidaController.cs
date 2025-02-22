using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.User.Infraestructure.Pipeline.Middleware.Attributes;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Authorize]

[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UnidadDeMedidaController(IUnidadDeMedidaQueryService queryService, IUnidadDeMedidaCommandService commandService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUnidadesDeMedida()
    {
        var query = new GetAllUnidadesDeMedidaQuery();
        var unidades = await queryService.Handle(query);
        return Ok(unidades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUnidadDeMedidaById(int id)
    {
        var query = new GetUnidadDeMedidaByIdQuery(id);
        var unidad = await queryService.Handle(query);

        if (unidad == null) return NotFound("Unidad de Medida not found.");
        return Ok(unidad);
    }

    [HttpPost("createUnidadDeMedida")]
    public async Task<IActionResult> CreateUnidadDeMedida([FromBody] CrearUnidadDeMedidaCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        var id = await commandService.Handle(command);
        var unidad = await queryService.Handle(new GetUnidadDeMedidaByIdQuery(id));
        
        if (unidad == null) return NotFound("Unidad de Medida not found.");

        return CreatedAtAction(nameof(GetUnidadDeMedidaById), new { id }, unidad);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUnidadDeMedida(int id, [FromBody] ActualizarUnidadDeMedidaCommand command)
    {
        if (command == null || id != command.Id) return BadRequest("Invalid input.");

        var success = await commandService.Handle(command);
        if (!success) return NotFound("Unidad de Medida not found.");

        return NoContent();
    }
}