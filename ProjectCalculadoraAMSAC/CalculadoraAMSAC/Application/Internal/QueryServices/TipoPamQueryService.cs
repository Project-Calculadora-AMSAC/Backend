using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;


public class TipoPamQueryService(ITipoPamRepository tipoPamRepository) : ITipoPamQueryService
{
    public async Task<List<TipoPam>> Handle(GetAllTipoPamQuery query)
    {
        return await tipoPamRepository.GetAllAsync();
    }

    public async Task<TipoPam?> Handle(GetTipoPamByIdQuery query)
    {
        return await tipoPamRepository.GetByIdAsync(query.Id);
    }
}