using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IProyectoCommandService
{
    Task<int> Handle(CrearProyectoCommand command);
    Task<bool> Handle(ActualizarProyectoCommand command);
    Task<Proyecto?> Handle(ActualizarEstadoProyectoCommand command);
}