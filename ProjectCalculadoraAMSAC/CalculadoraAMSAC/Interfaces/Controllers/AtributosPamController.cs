using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AtributosPamController(IAtributoPamQueryService queryService, IAtributoPamCommandService commandService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAtributosPam()
    {
        var query = new GetAllAtributosPamQuery();
        var atributosPam = await queryService.Handle(query);
        return Ok(atributosPam);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAtributoPamById(int id)
    {
        var query = new GetAtributoPamByIdQuery(id);
        var atributoPam = await queryService.Handle(query);

        if (atributoPam == null) return NotFound("AtributoPam not found.");

        return Ok(atributoPam);
    }
    
    
    [HttpGet("atributos/{tipoPamid}")]
    public async Task<IActionResult> GetAtributoPamByTipoPamId(int tipoPamid)
    {
        var query = new GetAtributosPamByTipoPamIdQuery(tipoPamid);
        var atributoPam = await queryService.Handle(query);

        if (atributoPam == null) return NotFound("AtributoPam not found.");

        return Ok(atributoPam);
    }
    
    [HttpPost("createAtributoPam")]
    public async Task<IActionResult> CreateAtributoPam([FromBody] CrearAtributoPamCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        var id = await commandService.Handle(command);
        var atributoPam = await queryService.Handle(new GetAtributoPamByIdQuery(id));

        if (atributoPam == null) return NotFound("AtributoPam not found.");

        return CreatedAtAction(nameof(GetAtributoPamById), new { id }, atributoPam);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAtributoPam(int id, [FromBody] ActualizarAtributoPamCommand command)
    {
        if (command == null || id != command.AtributoPamId) return BadRequest("Invalid input.");

        var success = await commandService.Handle(command);
        if (!success) return NotFound("AtributoPam not found.");

        return NoContent();
    }
}