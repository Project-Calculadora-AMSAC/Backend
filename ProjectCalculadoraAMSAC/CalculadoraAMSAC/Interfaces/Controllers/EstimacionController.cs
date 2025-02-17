using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.ValueObject;
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

        // ✅ Calcular el costo estimado
        var costoEstimado = new CostoEstimado(estimacion);

        // ✅ Devolver la estimación junto con el costo estimado
        return Ok(new
        {
            EstimacionId = estimacion.EstimacionId,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            TipoPam = estimacion.TipoPam,
            Valores = estimacion.Valores,
            CostoEstimado = new
            {
                CostoDirecto = costoEstimado.CostoDirecto,
                GastosGenerales = costoEstimado.GastosGenerales,
                Utilidades = costoEstimado.Utilidades,
                IGV = costoEstimado.IGV,
                ExpedienteTecnico = costoEstimado.ExpedienteTecnico,
                Supervision = costoEstimado.Supervision,
                GestionProyecto = costoEstimado.GestionProyecto,
                Capacitacion = costoEstimado.Capacitacion,
                Contingencias = costoEstimado.Contingencias,
                SubTotal = costoEstimado.SubTotal,
                SubTotalObras = costoEstimado.SubTotalObras,
                TotalEstimado = costoEstimado.TotalEstimado
            }
        });
    }
    
    [HttpPost("createEstimacion")]
    public async Task<IActionResult> CreateEstimacion([FromBody] CrearEstimacionCommand command)
    {
        if (command == null) return BadRequest("Invalid input.");

        // ✅ Crear la estimación en la base de datos
        var id = await commandService.Handle(command);

        // ✅ Obtener la estimación recién creada para calcular el costo
        var estimacion = await queryService.Handle(new GetEstimacionByIdQuery(id));
        if (estimacion == null) return NotFound("Estimation not found.");

        // ✅ Calcular el costo estimado
        var costoEstimado = new CostoEstimado(estimacion);

        // ✅ Devolver la estimación junto con el costo estimado
        return CreatedAtAction(nameof(GetEstimacionById), new { id }, new
        {
            EstimacionId = id,
            UsuarioId = estimacion.UsuarioId,
            Proyecto = estimacion.Proyecto,
            TipoPam = estimacion.TipoPam,
            Valores = estimacion.Valores,
            CostoEstimado = new
            {
                CostoDirecto = costoEstimado.CostoDirecto,
                GastosGenerales = costoEstimado.GastosGenerales,
                Utilidades = costoEstimado.Utilidades,
                IGV = costoEstimado.IGV,
                ExpedienteTecnico = costoEstimado.ExpedienteTecnico,
                Supervision = costoEstimado.Supervision,
                GestionProyecto = costoEstimado.GestionProyecto,
                Capacitacion = costoEstimado.Capacitacion,
                Contingencias = costoEstimado.Contingencias,
                SubTotal = costoEstimado.SubTotal,
                SubTotalObras = costoEstimado.SubTotalObras,
                TotalEstimado = costoEstimado.TotalEstimado
            }
        });
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