using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IEstimacionCommandService
{
    Task<int> Handle(CrearEstimacionCommand command);
    Task<bool> Handle(ActualizarEstimacionCommand command);
}
