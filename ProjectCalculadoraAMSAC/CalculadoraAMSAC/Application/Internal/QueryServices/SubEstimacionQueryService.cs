using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices
{
    public class SubEstimacionQueryService(ISubEstimacionRepository subEstimacionRepository) : ISubEstimacionQueryService
    {
        public async Task<SubEstimacion?> Handle(GetSubEstimacionByIdQuery query)
        {
            return await subEstimacionRepository.GetByIdAsync(query.Id);
        }

        public async Task<List<SubEstimacion>> Handle(GetSubEstimacionesByEstimacionIdQuery query)
        {
            return await subEstimacionRepository.GetByEstimacionIdAsync(query.EstimacionId);
        }
    }
}