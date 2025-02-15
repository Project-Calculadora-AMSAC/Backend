using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EstimacionController(IEstimacionQueryService queryService, IEstimacionCommandService commandService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllEstimaciones()
    {
        var query = new GetAllEstimacionesQuery();
        var estimaciones = await queryService.Handle(query);
        return Ok(estimaciones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEstimacionById(int id)
    {
        var query = new GetEstimacionByIdQuery(id);
        var estimacion = await queryService.Handle(query);
        if (estimacion == null) return NotFound("Estimation not found.");
        return Ok(estimacion);
    }
    
    public async Task<IActionResult> CreateEstimacion([FromBody] CrearEstimacionCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");
        var id = await commandService.Handle(command);
        return CreatedAtAction(nameof(GetEstimacionById), new { id }, new { EstimacionId = id });
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEstimacion(int id, [FromBody] ActualizarEstimacionCommand command)
    {
        if (command == null || id != command.EstimacionId) return BadRequest("Invalid input.");
        var success = await commandService.Handle(command);
        if (!success) return NotFound("Estimation not found.");
        return NoContent();
    }
}