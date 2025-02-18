using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class EstimacionCommandService : IEstimacionCommandService
{
    private readonly IEstimacionRepository _estimacionRepository;
    private readonly ICostoEstimadoRepository _costoEstimadoRepository;
    private readonly ITipoPamRepository _tipoPamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EstimacionCommandService(
        IEstimacionRepository estimacionRepository,
        ITipoPamRepository tipoPamRepository,
        ICostoEstimadoRepository costoEstimadoRepository,
        IUnitOfWork unitOfWork)
    {
        _estimacionRepository = estimacionRepository;
        _tipoPamRepository = tipoPamRepository;
        _costoEstimadoRepository = costoEstimadoRepository;
        _unitOfWork = unitOfWork;
    }

  public async Task<int> Handle(CrearEstimacionCommand command)
{
    if (command == null)
        throw new ArgumentException("Invalid estimation data.");

    // ✅ Buscar TipoPam antes de crear la estimación
    var tipoPam = await _tipoPamRepository.GetByIdWithVariablesAsync(command.TipoPamId);

    if (tipoPam == null)
        throw new InvalidOperationException($"No se encontró el TipoPam con ID {command.TipoPamId}.");

    if (tipoPam.Variables == null || !tipoPam.Variables.Any())
    {
        Console.WriteLine($"ERROR: TipoPam {command.TipoPamId} no tiene variables asignadas.");
        throw new InvalidOperationException($"El TipoPam con ID {command.TipoPamId} no tiene variables asignadas.");
    }

    // ✅ Crear instancia de Estimacion
    var nuevaEstimacion = new Estimacion(
        command.UsuarioId,
        command.ProyectoId,
        tipoPam.Id,
        command.CodPam,
        command.Valores
    );

    try
    {
        // ✅ Guardar la estimación antes de calcular costos
        await _estimacionRepository.AddAsync(nuevaEstimacion);
        await _unitOfWork.CompleteAsync();  // Esto garantiza que la estimación tenga un ID

        Console.WriteLine($"DEBUG: Estimación guardada con ID {nuevaEstimacion.EstimacionId}");

        // ✅ Recuperar la estimación desde la BD para asegurarse de que tiene el ID y las relaciones correctas
        var estimacionGuardada = await _estimacionRepository.GetByIdAsync(nuevaEstimacion.EstimacionId);
        if (estimacionGuardada == null)
            throw new Exception($"No se pudo recuperar la estimación con ID {nuevaEstimacion.EstimacionId}.");

        Console.WriteLine($"DEBUG: Estimación recuperada con ID {estimacionGuardada.EstimacionId} y TipoPam {estimacionGuardada.TipoPam?.Name}");

        // ✅ Crear `CostoEstimado` basado en la estimación guardada
        var costoEstimado = new CostoEstimado(estimacionGuardada);
        await _costoEstimadoRepository.AddAsync(costoEstimado);
        await _unitOfWork.CompleteAsync();

        return estimacionGuardada.EstimacionId;
    }
    catch (Exception e)
    {
        Console.WriteLine($"ERROR: {e.Message}");
        throw new Exception($"An error occurred while creating the estimation: {e.Message}");
    }
}

    public async Task<bool> Handle(ActualizarEstimacionCommand command)
    {
        try
        {
            var estimacion = await _estimacionRepository.GetByIdAsync(command.EstimacionId);
            if (estimacion == null)
                throw new Exception("Estimation not found.");

            // ✅ Actualizar valores de la estimación
            estimacion.ActualizarValores(new Dictionary<int, string>(command.Valores));

            // ✅ Obtener el costo asociado y recalcularlo
            var costoEstimado = await _costoEstimadoRepository.GetByEstimacionId(command.EstimacionId);
            if (costoEstimado != null)
            {
                costoEstimado.CalcularCostos(estimacion);
                await _unitOfWork.CompleteAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR: {e.Message}");
            throw new Exception($"An error occurred while updating the estimation: {e.Message}");
        }
    }
}
