using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class VariablesPamCommandService : IVariablesPamCommandService
{
    private readonly IVariablesPamRepository _variablesPamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VariablesPamCommandService(IVariablesPamRepository variablesPamRepository, IUnitOfWork unitOfWork)
    {
        _variablesPamRepository = variablesPamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearVariablesPamCommand command)
    {
        if (command == null)
            throw new ArgumentException("Invalid VariablesPam data.");

        var nuevaVariable = new VariablesPam(command.TipoPamId, command.Nombre, command.Valor);

        try
        {
            await _variablesPamRepository.AddAsync(nuevaVariable);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while creating VariablesPam: {e.Message}");
        }

        return nuevaVariable.Id;
    }

    public async Task<bool> Handle(ActualizarVariablesPamCommand command)
    {
        var variable = await _variablesPamRepository.GetByIdAsync(command.Id);
        if (variable == null) return false;

        variable.Nombre = command.Nombre;
        variable.Valor = command.Valor;

        try
        {
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"An error occurred while updating VariablesPam: {e.Message}");
        }
    }
}