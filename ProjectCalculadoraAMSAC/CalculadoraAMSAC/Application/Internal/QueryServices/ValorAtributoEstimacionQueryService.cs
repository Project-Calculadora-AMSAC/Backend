using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class ValorAtributoEstimacionQueryService : IValorAtributoEstimacionQueryService
{
    private readonly IValorAtributoEstimacionRepository _valorAtributoEstimacionRepository;

    public ValorAtributoEstimacionQueryService(IValorAtributoEstimacionRepository valorAtributoEstimacionRepository)
    {
        _valorAtributoEstimacionRepository = valorAtributoEstimacionRepository;
    }

    public async Task<IEnumerable<ValorAtributoEstimacion>> Handle(GetAllValoresAtributoEstimacionQuery query)
    {
        return await _valorAtributoEstimacionRepository.GetAllAsync();
    }

    public async Task<ValorAtributoEstimacion?> Handle(GetValorAtributoEstimacionByIdQuery query)
    {
        return await _valorAtributoEstimacionRepository.GetByIdAsync(query.Id);
    }
}