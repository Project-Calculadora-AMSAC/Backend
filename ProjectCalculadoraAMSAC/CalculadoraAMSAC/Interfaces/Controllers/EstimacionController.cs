using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.User.Infraestructure.Pipeline.Middleware.Attributes;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers;

[ApiController]

[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class EstimacionController : ControllerBase
{
    private readonly IEstimacionQueryService _queryService;
    private readonly IEstimacionCommandService _commandService;

    public EstimacionController(IEstimacionQueryService queryService, IEstimacionCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    // ✅ Obtener todas las estimaciones con su costo asociado
    [HttpGet]
    public async Task<IActionResult> GetAllEstimaciones()
    {
        var query = new GetAllEstimacionesQuery();
        var estimaciones = await _queryService.Handle(query);

        if (estimaciones == null || !estimaciones.Any()) return NotFound("No estimations found.");

        return Ok(estimaciones.Select(estimacion => new
        {
            EstimacionId = estimacion.EstimacionId,
            UsuarioId = estimacion.UsuarioId,
            CodPam = estimacion.CodPam,
            Proyecto = estimacion.Proyecto,
            TipoPam = estimacion.TipoPam,
            Valores = estimacion.Valores,
            CostoEstimado = estimacion.CostoEstimado != null ? new
            {
                CostoDirecto = estimacion.CostoEstimado.CostoDirecto,
                GastosGenerales = estimacion.CostoEstimado.GastosGenerales,
                Utilidades = estimacion.CostoEstimado.Utilidades,
                IGV = estimacion.CostoEstimado.IGV,
                ExpedienteTecnico = estimacion.CostoEstimado.ExpedienteTecnico,
                Supervision = estimacion.CostoEstimado.Supervision,
                GestionProyecto = estimacion.CostoEstimado.GestionProyecto,
                Capacitacion = estimacion.CostoEstimado.Capacitacion,
                Contingencias = estimacion.CostoEstimado.Contingencias,
                SubTotal = estimacion.CostoEstimado.SubTotal,
                SubTotalObras = estimacion.CostoEstimado.SubTotalObras,
                TotalEstimado = estimacion.CostoEstimado.TotalEstimado
            } : null
        }));
    }

    // ✅ Obtener una estimación por ID con su costo
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEstimacionById(int id)
    {
        var query = new GetEstimacionByIdQuery(id);
        var estimacion = await _queryService.Handle(query);

        if (estimacion == null) return NotFound("Estimation not found.");

        
        return Ok(new
        {
            EstimacionId = estimacion.EstimacionId,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            TipoPam = estimacion.TipoPam,
            Valores = estimacion.Valores,
            CostoEstimado = estimacion.CostoEstimado != null ? new
            {
                CostoDirecto = estimacion.CostoEstimado.CostoDirecto,
                GastosGenerales = estimacion.CostoEstimado.GastosGenerales,
                Utilidades = estimacion.CostoEstimado.Utilidades,
                IGV = estimacion.CostoEstimado.IGV,
                ExpedienteTecnico = estimacion.CostoEstimado.ExpedienteTecnico,
                Supervision = estimacion.CostoEstimado.Supervision,
                GestionProyecto = estimacion.CostoEstimado.GestionProyecto,
                Capacitacion = estimacion.CostoEstimado.Capacitacion,
                Contingencias = estimacion.CostoEstimado.Contingencias,
                SubTotal = estimacion.CostoEstimado.SubTotal,
                SubTotalObras = estimacion.CostoEstimado.SubTotalObras,
                TotalEstimado = estimacion.CostoEstimado.TotalEstimado
            } : null
        });
    }

  [HttpPost("createEstimacion")]
public async Task<IActionResult> CreateEstimacion([FromBody] CrearEstimacionResource resource)
{
    if (resource == null) return BadRequest("Invalid input.");

    var command = CrearEstimacionTransform.ToCommand(resource);
    Console.WriteLine($"DEBUG: Enviando comando para crear estimación con TipoPamId {command.TipoPamId}");

    // ✅ Ahora esperamos la confirmación del guardado antes de continuar
    var id = await _commandService.Handle(command);

    if (id == 0)
    {
        Console.WriteLine("ERROR: La estimación fue creada pero el ID sigue siendo 0.");
        return StatusCode(500, "Failed to create estimation.");
    }

    Console.WriteLine($"DEBUG: Estimación creada con ID {id}, consultando detalles...");

    var estimacion = await _queryService.Handle(new GetEstimacionByIdQuery(id));
    if (estimacion == null) return NotFound("Estimation not found.");

    return CreatedAtAction(nameof(GetEstimacionById), new { id }, new
    {
        EstimacionId = id,
        UsuarioId = estimacion.UsuarioId,
        Proyecto = estimacion.Proyecto,
        TipoPam = estimacion.TipoPam,
        Valores = estimacion.Valores,
        CostoEstimado = estimacion.CostoEstimado != null ? new
        {
            CostoDirecto = estimacion.CostoEstimado.CostoDirecto,
            GastosGenerales = estimacion.CostoEstimado.GastosGenerales,
            Utilidades = estimacion.CostoEstimado.Utilidades,
            IGV = estimacion.CostoEstimado.IGV,
            ExpedienteTecnico = estimacion.CostoEstimado.ExpedienteTecnico,
            Supervision = estimacion.CostoEstimado.Supervision,
            GestionProyecto = estimacion.CostoEstimado.GestionProyecto,
            Capacitacion = estimacion.CostoEstimado.Capacitacion,
            Contingencias = estimacion.CostoEstimado.Contingencias,
            SubTotal = estimacion.CostoEstimado.SubTotal,
            SubTotalObras = estimacion.CostoEstimado.SubTotalObras,
            TotalEstimado = estimacion.CostoEstimado.TotalEstimado
        } : null
    });
}



    // ✅ Actualizar una estimación existente
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEstimacion(int id, [FromBody] ActualizarEstimacionCommand command)
    {
        if (command == null || id != command.EstimacionId) return BadRequest("Invalid input.");
        
        var success = await _commandService.Handle(command);
        if (!success) return NotFound("Estimation not found.");

        return NoContent();
    }

    // ✅ Obtener el costo total de todas las estimaciones de un proyecto
    [HttpGet("totalCost/{proyectoId}")]
    public async Task<IActionResult> GetTotalCost(int proyectoId)
    {
        var query = new GetTotalCostByProjectIdQuery(proyectoId);

        var totalCost = await _queryService.Handle(query);
        return Ok(new { ProyectoId = proyectoId, TotalCost = totalCost });
    }
    [HttpGet("proyecto/{proyectoId}")]
    public async Task<IActionResult> GetEstimacionesByProyectoId(int proyectoId)
    {
        var query = new GetEstimacionesByProyectoIdQuery(proyectoId);
        var estimaciones = await _queryService.Handle(query);

        if (estimaciones == null || !estimaciones.Any())
            return NotFound("No estimaciones found for this project.");

        return Ok(estimaciones);
    }

    [HttpGet("tipopam/{tipoPamId}")]
    public async Task<IActionResult> GetEstimacionesByTipoPamId(int tipoPamId)
    {
        var query = new GetEstimacionesByTipoPamIdQuery(tipoPamId);
        var estimaciones = await _queryService.Handle(query);

        if (estimaciones == null || !estimaciones.Any())
            return NotFound("No estimaciones found for this TipoPam.");

        return Ok(estimaciones);
    }
    
    [HttpGet("buscar")]
    public async Task<IActionResult> GetEstimaciones(int proyectoId, int tipoPamId)
    {
        var query = new GetEstimacionesByProyectoIdAndTipoPamIdQuery(proyectoId, tipoPamId);
        var estimaciones = await _queryService.Handle(query);

        if (estimaciones == null || !estimaciones.Any())
            return NotFound("No estimaciones found with the given filters.");

        return Ok(estimaciones);
    }
}
