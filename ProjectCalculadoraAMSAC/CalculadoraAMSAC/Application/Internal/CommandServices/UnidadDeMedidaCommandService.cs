using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class UnidadDeMedidaCommandService : IUnidadDeMedidaCommandService
{
    private readonly IUnidadDeMedidaRepository _unidadDeMedidaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnidadDeMedidaCommandService(IUnidadDeMedidaRepository unidadDeMedidaRepository, IUnitOfWork unitOfWork)
    {
        _unidadDeMedidaRepository = unidadDeMedidaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearUnidadDeMedidaCommand command)
    {
        var nuevaUnidad = new UnidadDeMedida(command.Nombre, command.Simbolo);
        
        await _unidadDeMedidaRepository.AddAsync(nuevaUnidad);
        await _unitOfWork.CompleteAsync();
        
        return nuevaUnidad.Id;
    }

    public async Task<bool> Handle(ActualizarUnidadDeMedidaCommand command)
    {
        var unidad = await _unidadDeMedidaRepository.GetByIdAsync(command.Id);
        if (unidad == null) return false;

        unidad.Nombre = command.Nombre;
        unidad.Simbolo = command.Simbolo;
        
        await _unitOfWork.CompleteAsync();
        return true;
    }
    
}