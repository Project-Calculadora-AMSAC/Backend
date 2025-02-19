using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IVariablesPamCommandService
{
    Task<int> Handle(CrearVariablesPamCommand command);
    Task<bool> Handle(ActualizarVariablesPamCommand command);
}