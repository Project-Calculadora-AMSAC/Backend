using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface ITipoPamCommandService
{
    Task<int> Handle(CrearTipoPamCommand command);
    Task<bool> Handle(ActualizarTipoPamCommand command);
}