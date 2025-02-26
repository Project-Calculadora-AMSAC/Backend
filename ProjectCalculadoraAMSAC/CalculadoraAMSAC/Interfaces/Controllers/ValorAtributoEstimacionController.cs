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
public class ValorAtributoEstimacionController(
    IValorAtributoEstimacionQueryService queryService,
    IValorAtributoEstimacionCommandService commandService)
    : ControllerBase
{
    // ✅ Obtener todos los valores de una SubEstimación
    [HttpGet("subestimacion/{subEstimacionId}")]
    public async Task<IActionResult> GetAllBySubEstimacionId(int subEstimacionId)
    {
        var query = new GetAllValoresAtributoSubEstimacionQuery(subEstimacionId);
        var valores = await queryService.Handle(query);

        if (valores is null || !valores.Any()) return NotFound("No values found for the given SubEstimacionId.");

        return Ok(valores);
    }

    // ✅ Obtener un valor por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetValorAtributoEstimacionByIdQuery(id);
        var valor = await queryService.Handle(query);

        if (valor == null) return NotFound("ValorAtributoEstimacion not found.");

        return Ok(valor);
    }

    // ✅ Crear un nuevo valor para una SubEstimación
    [HttpPost("createValorAtributoEstimacion")]
    public async Task<IActionResult> Create([FromBody] CrearValorAtributoSubEstimacionCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        var id = await commandService.Handle(command);
        var valor = await queryService.Handle(new GetValorAtributoEstimacionByIdQuery(id));

        if (valor == null) return NotFound("ValorAtributoEstimacion not found.");

        return CreatedAtAction(nameof(GetById), new { id }, valor);
    }

    // ✅ Actualizar un valor de una SubEstimación
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarValorAtributoEstimacionCommand command)
    {
        if (command == null || id != command.Id) return BadRequest("Invalid input.");

        var success = await commandService.Handle(command);
        if (!success) return NotFound("ValorAtributoEstimacion not found.");

        return NoContent();
    }
}
