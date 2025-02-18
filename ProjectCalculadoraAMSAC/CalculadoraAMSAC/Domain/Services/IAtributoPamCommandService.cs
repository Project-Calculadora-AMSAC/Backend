using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IAtributoPamCommandService
{
    Task<int> Handle(CrearAtributoPamCommand command);
    Task<bool> Handle(ActualizarAtributoPamCommand command);
}