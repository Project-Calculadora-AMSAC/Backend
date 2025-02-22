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
public class TipoPamController(ITipoPamQueryService queryService, ITipoPamCommandService commandService)
    : ControllerBase
{
    [HttpGet("tipoPams")]
    public async Task<IActionResult> GetAllTipoPams()
    {
        var query = new GetAllTipoPamQuery();
        var tipoPams = await queryService.Handle(query);
        return Ok(tipoPams);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTipoPamById(int id)
    {
        var query = new GetTipoPamByIdQuery(id);
        var tipoPam = await queryService.Handle(query);
        if (tipoPam == null) return NotFound("TipoPam not found.");
        return Ok(tipoPam);
    }

    [HttpPost("createTipoPam")]
    public async Task<IActionResult> CreateTipoPam([FromBody] CrearTipoPamCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");
        var id = await commandService.Handle(command);
        return CreatedAtAction(nameof(GetTipoPamById), new { id }, new { TipoPamId = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTipoPam(int id, [FromBody] ActualizarTipoPamCommand command)
    {
        if (command == null || id != command.Id) return BadRequest("Invalid input.");
        var success = await commandService.Handle(command);
        if (!success) return NotFound("TipoPam not found.");
        return NoContent();
    }
}
