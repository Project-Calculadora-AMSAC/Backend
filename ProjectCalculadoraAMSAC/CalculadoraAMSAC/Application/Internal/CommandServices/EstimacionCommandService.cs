using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class EstimacionCommandService : IEstimacionCommandService
{
    private readonly IEstimacionRepository _estimacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EstimacionCommandService(IEstimacionRepository estimacionRepository, IUnitOfWork unitOfWork)
    {
        _estimacionRepository = estimacionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearEstimacionCommand command)
    {
        if (command == null)
        {
            throw new ArgumentException("Invalid estimation data.");
        }

        var nuevaEstimacion = new Estimacion(
            command.UsuarioId,
            command.ProyectoId,
            command.TipoPamId,
            command.CodPam,
            command.Valores // Este debe ser un Dictionary<int, string> como lo requiere el constructor
        );
        try
        {
            await _estimacionRepository.AddAsync(nuevaEstimacion);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while creating the estimation: {e.Message}");
        }

        return nuevaEstimacion.EstimacionId;
    }

    public async Task<bool> Handle(ActualizarEstimacionCommand command)
    {
        try
        {
            var estimacion = await _estimacionRepository.GetByIdAsync(command.EstimacionId);
            if (estimacion == null)
                throw new Exception("Estimation not found.");

            // Aquí actualizas los valores necesarios
            estimacion.ActualizarValores(new Dictionary<int, string>(command.Valores));

            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while updating the estimation: {e.Message}");
        }
    }
}