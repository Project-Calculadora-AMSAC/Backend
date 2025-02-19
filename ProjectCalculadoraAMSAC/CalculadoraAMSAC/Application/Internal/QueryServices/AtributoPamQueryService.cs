using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class AtributoPamQueryService : IAtributoPamQueryService
{
    private readonly IAtributoPamRepository _atributoPamRepository;

    public AtributoPamQueryService(IAtributoPamRepository atributoPamRepository)
    {
        _atributoPamRepository = atributoPamRepository;
    }

    public async Task<AtributosPam> Handle(GetAtributoPamByIdQuery query)
    {
        return await _atributoPamRepository.GetByIdAsync(query.Id);
    }

    public async Task<List<AtributosPam>> Handle(GetAllAtributosPamQuery query)
    {
        return await _atributoPamRepository.GetAllAsync();
    }

    public async Task<List<AtributosPam>> Handle(GetAtributosPamByTipoPamIdQuery query)
    {
        return await _atributoPamRepository.GetAllByTipoPamAsync(query.TipoPamId);
    }
}