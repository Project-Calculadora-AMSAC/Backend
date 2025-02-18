using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class UnidadDeMedidaQueryService : IUnidadDeMedidaQueryService
{
    private readonly IUnidadDeMedidaRepository _unidadDeMedidaRepository;

    public UnidadDeMedidaQueryService(IUnidadDeMedidaRepository unidadDeMedidaRepository)
    {
        _unidadDeMedidaRepository = unidadDeMedidaRepository;
    }

    public async Task<UnidadDeMedida> Handle(GetUnidadDeMedidaByIdQuery query)
    {
        return await _unidadDeMedidaRepository.GetByIdAsync(query.Id);
    }

    public async Task<List<UnidadDeMedida>> Handle(GetAllUnidadesDeMedidaQuery query)
    {
        return await _unidadDeMedidaRepository.GetAllAsync();
    }
}
