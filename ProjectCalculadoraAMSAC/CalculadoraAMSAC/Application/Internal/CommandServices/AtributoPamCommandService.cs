using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class AtributoPamCommandService : IAtributoPamCommandService
{
    private readonly IAtributoPamRepository _atributoPamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtributoPamCommandService(IAtributoPamRepository atributoPamRepository, IUnitOfWork unitOfWork)
    {
        _atributoPamRepository = atributoPamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearAtributoPamCommand command)
    {
        if (command == null)
        {
            throw new ArgumentException("Invalid AtributoPam data.");
        }

        var nuevoAtributoPam = new AtributosPam(
            command.TipoPamId,
            command.UnidadDeMedidaId,
            command.Nombre,
            command.TipoDato
        );

        try
        {
            await _atributoPamRepository.AddAsync(nuevoAtributoPam);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while creating the AtributoPam: {e.Message}");
        }

        return nuevoAtributoPam.AtributoPamId;
    }

    public async Task<bool> Handle(ActualizarAtributoPamCommand command)
    {
        try
        {
            var atributoPam = await _atributoPamRepository.GetByIdAsync(command.AtributoPamId);
            if (atributoPam == null)
                throw new Exception("AtributoPam not found.");

            atributoPam.Nombre = command.Nombre;
            atributoPam.TipoDato = command.TipoDato;
            atributoPam.UnidadDeMedidaId = command.UnidadDeMedidaId;

            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while updating the AtributoPam: {e.Message}");
        }
    }
}