using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class EstimacionQueryService(IEstimacionRepository estimacionRepository) : IEstimacionQueryService
{
    public async Task<Estimacion?> Handle(GetEstimacionByIdQuery query)
    {
        return await estimacionRepository.GetByIdAsync(query.EstimacionId);
    }

    public async Task<IEnumerable<Estimacion>> Handle(GetAllEstimacionesQuery query)
    {
        return await estimacionRepository.GetAllAsync();
    }
}