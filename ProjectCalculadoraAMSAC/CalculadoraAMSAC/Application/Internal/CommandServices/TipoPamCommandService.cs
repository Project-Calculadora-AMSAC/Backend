using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class TipoPamCommandService : ITipoPamCommandService
{
    private readonly ITipoPamRepository _tipoPamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TipoPamCommandService(ITipoPamRepository tipoPamRepository, IUnitOfWork unitOfWork)
    {
        _tipoPamRepository = tipoPamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearTipoPamCommand command)
    {
        if (command == null)
        {
            throw new ArgumentException("Invalid TipoPam data.");
        }

        var nuevoTipoPam = new TipoPam(command.Name, command.Status);
        
        try
        {
            await _tipoPamRepository.AddAsync(nuevoTipoPam);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while creating the TipoPam: {e.Message}");
        }

        return nuevoTipoPam.Id;
    }

    public async Task<bool> Handle(ActualizarTipoPamCommand command)
    {
        try
        {
            var tipoPam = await _tipoPamRepository.GetByIdAsync(command.Id);
            if (tipoPam == null)
                throw new Exception("TipoPam not found.");

            // ✅ Actualizar valores
            tipoPam.Name = command.Name;
            tipoPam.Status = command.Status;

            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while updating the TipoPam: {e.Message}");
        }
    }
}