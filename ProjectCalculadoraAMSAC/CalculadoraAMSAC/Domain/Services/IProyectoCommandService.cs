using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IProyectoCommandService
{
    Task<int> Handle(CrearProyectoCommand command);
    Task<bool> Handle(ActualizarProyectoCommand command);
}