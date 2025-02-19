using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class ValorAtributoEstimacionCommandService : IValorAtributoEstimacionCommandService
{
    private readonly IValorAtributoEstimacionRepository _valorAtributoEstimacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ValorAtributoEstimacionCommandService(
        IValorAtributoEstimacionRepository valorAtributoEstimacionRepository,
        IUnitOfWork unitOfWork)
    {
        _valorAtributoEstimacionRepository = valorAtributoEstimacionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearValorAtributoEstimacionCommand command)
    {
        if (command == null)
        {
            throw new ArgumentException("Invalid data for value attribute estimation.");
        }

        var nuevoValorAtributo = new ValorAtributoEstimacion(
            command.EstimacionId,
            command.AtributoPamId,
            command.Valor
        );

        try
        {
            await _valorAtributoEstimacionRepository.AddAsync(nuevoValorAtributo);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while creating the value attribute estimation: {e.Message}");
        }

        return nuevoValorAtributo.Id;
    }

    public async Task<bool> Handle(ActualizarValorAtributoEstimacionCommand command)
    {
        try
        {
            var valorAtributo = await _valorAtributoEstimacionRepository.GetByIdAsync(command.Id);
            if (valorAtributo == null)
                throw new Exception("Value attribute estimation not found.");

            valorAtributo.ActualizarValor(command.Valor);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while updating the value attribute estimation: {e.Message}");
        }
    }
}