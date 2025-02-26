using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]
[Authorize]
[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EstimacionController : ControllerBase
{
    private readonly IEstimacionQueryService _queryService;
    private readonly IEstimacionCommandService _commandService;
    private readonly ISubEstimacionQueryService _subQueryService;
    private readonly ISubEstimacionCommandService _subCommandService;

    public EstimacionController(
        IEstimacionQueryService queryService,
        IEstimacionCommandService commandService,
        ISubEstimacionQueryService subQueryService,
        ISubEstimacionCommandService subCommandService)
    {
        _queryService = queryService;
        _commandService = commandService;
        _subQueryService = subQueryService;
        _subCommandService = subCommandService;
    }

    // ✅ Obtener todas las estimaciones con sus subestimaciones
    [HttpGet]
    public async Task<IActionResult> GetAllEstimaciones()
    {
        var estimaciones = await _queryService.Handle(new GetAllEstimacionesQuery());
        if (estimaciones == null || !estimaciones.Any()) 
            return NotFound("No estimations found.");

        return Ok(estimaciones.Select(estimacion => new
        {
            EstimacionId = estimacion.EstimacionId,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            FechaEstimacion = estimacion.FechaEstimacion,
            SubEstimaciones = estimacion.SubEstimaciones?.Select(sub => new
            {
                SubEstimacionId = sub.Id,
                TipoPam = sub.TipoPam,
                Cantidad = sub.Cantidad,
                Valores = sub.Valores?.ToDictionary(v => v.Id, v => v.Valor ?? string.Empty) 
                          ?? new Dictionary<int, string>(),
                CostoEstimado = sub.CostoEstimado?.TotalEstimado ?? 0
            })
        }));
    }

    // ✅ Obtener una estimación por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEstimacionById(int id)
    {
        var estimacion = await _queryService.Handle(new GetEstimacionByIdQuery(id));
        if (estimacion == null) return NotFound("Estimation not found.");

        return Ok(new
        {
            EstimacionId = estimacion.EstimacionId,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            FechaEstimacion = estimacion.FechaEstimacion,
            SubEstimaciones = estimacion.SubEstimaciones?.Select(sub => new
            {
                SubEstimacionId = sub.Id,
                TipoPam = sub.TipoPam,
                Cantidad = sub.Cantidad,
                Valores = sub.Valores?.ToDictionary(v => v.Id, v => v.Valor ?? string.Empty) 
                          ?? new Dictionary<int, string>(),
                CostoEstimado = sub.CostoEstimado.TotalEstimado
            }) 
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEstimacion([FromBody] CrearEstimacionResource resource)
    {
        if (resource == null || resource.SubEstimaciones == null || !resource.SubEstimaciones.Any())
            return BadRequest("Invalid input: At least one subestimación is required.");

        Console.WriteLine($"DEBUG: Recibida petición para crear estimación con {resource.SubEstimaciones.Count} subestimaciones.");

        // Transformar el recurso en comando
        var command = CrearEstimacionTransform.ToCommand(resource);

        // Crear la estimación con subestimaciones incluidas
        var id = await _commandService.Handle(command);

        if (id == 0)
            return StatusCode(500, "Failed to create estimation.");

        // Obtener la estimación creada
        var estimacion = await _queryService.Handle(new GetEstimacionByIdQuery(id));
        if (estimacion == null)
            return NotFound("Estimation not found.");

        return CreatedAtAction(nameof(GetEstimacionById), new { id }, new
        {
            EstimacionId = id,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            FechaEstimacion = estimacion.FechaEstimacion,
            SubEstimaciones = estimacion.SubEstimaciones.Select(sub => new
            {
                SubEstimacionId = sub.Id,
                TipoPam = sub.TipoPam,
                Cantidad = sub.Cantidad,
                Valores = sub.Valores.ToDictionary(v => v.Id, v => v.Valor ?? string.Empty),
                CostoEstimado = sub.CostoEstimado?.TotalEstimado ?? 0
            }).ToList()
        });
    }


   

    // ✅ Eliminar una estimación
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEstimacion([FromRoute] int id)
    {
        var result = await _commandService.Handle(new EliminarEstimacionCommand(id));
        if (!result) return BadRequest("Failed to delete estimation.");
        return NoContent();
    }
}
