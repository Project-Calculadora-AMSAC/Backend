using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IValorAtributoEstimacionCommandService
{
    Task<int> Handle(CrearValorAtributoSubEstimacionCommand command);
    Task<bool> Handle(ActualizarValorAtributoEstimacionCommand command);
}