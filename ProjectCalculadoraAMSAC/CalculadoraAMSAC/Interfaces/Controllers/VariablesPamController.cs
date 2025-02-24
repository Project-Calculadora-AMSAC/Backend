using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Authorize]

[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class VariablesPamController(IVariablesPamQueryService queryService, IVariablesPamCommandService commandService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllVariablesPam()
    {
        var query = new GetAllVariablesPamQuery();
        var variables = await queryService.Handle(query);
        return Ok(variables);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVariablesPamById(int id)
    {
        var query = new GetVariablesPamByIdQuery(id);
        var variable = await queryService.Handle(query);

        if (variable == null) return NotFound("VariablePam not found.");
        
        return Ok(variable);
    }
    
    [HttpPost("createVariablesPam")]
    public async Task<IActionResult> CreateVariablesPam([FromBody] CrearVariablesPamCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        var id = await commandService.Handle(command);
        var variable = await queryService.Handle(new GetVariablesPamByIdQuery(id));
        
        if (variable == null) return NotFound("VariablePam creation failed.");
        
        return CreatedAtAction(nameof(GetVariablesPamById), new { id }, variable);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVariablesPam(int id, [FromBody] ActualizarVariablesPamCommand command)
    {
        if (command == null || id != command.Id) return BadRequest("Invalid input.");

        var success = await commandService.Handle(command);
        if (!success) return NotFound("VariablePam not found.");

        return NoContent();
    }
}