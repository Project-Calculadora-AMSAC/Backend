using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class ProyectoCommandService : IProyectoCommandService
{
    private readonly IProyectoRepository _proyectoRepository;

    public ProyectoCommandService(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<int> Handle(CrearProyectoCommand command)
    {
        var proyecto = new Proyecto(command.Name, command.Descripcion);
        await _proyectoRepository.AddAsync(proyecto);
        return proyecto.ProyectoId;
    }

    public async Task<bool> Handle(ActualizarProyectoCommand command)
    {
        var proyecto = await _proyectoRepository.GetByIdAsync(command.ProyectoId);
        if (proyecto == null) return false;

        proyecto.Name = command.Name;
        proyecto.Descripcion = command.Descripcion;
        proyecto.Estado = command.Estado;

        await _proyectoRepository.UpdateAsync(proyecto);
        return true;
    }

    public async Task<Proyecto?> Handle(ActualizarEstadoProyectoCommand command)
    {
        var proyecto = await _proyectoRepository.FindByIdAsync(command.IdProyecto);
        if (proyecto == null) return null;
        
        proyecto.DeleteProyecto();
        await _proyectoRepository.UpdateAsync(proyecto);
        return proyecto;
    }
}