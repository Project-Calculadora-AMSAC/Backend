using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
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

    public async Task<IEnumerable<ValorAtributoEstimacion>> Handle(GetAllValoresAtributoSubEstimacionQuery query)
    {
        return await _valorAtributoEstimacionRepository.GetAllBySubEstimacionIdAsync(query.SubEstimacionId);
    }

    public async Task<ValorAtributoEstimacion?> Handle(GetValorAtributoEstimacionByIdQuery query)
    {
        return await _valorAtributoEstimacionRepository.GetByIdAsync(query.Id);
    }
}