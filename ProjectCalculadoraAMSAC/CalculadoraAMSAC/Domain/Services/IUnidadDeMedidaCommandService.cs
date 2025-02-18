using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IUnidadDeMedidaCommandService
{
    Task<int> Handle(CrearUnidadDeMedidaCommand command);
    Task<bool> Handle(ActualizarUnidadDeMedidaCommand command);
}