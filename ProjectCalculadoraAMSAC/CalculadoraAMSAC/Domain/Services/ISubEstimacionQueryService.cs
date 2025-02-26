using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services
{
    public interface ISubEstimacionQueryService
    {
        Task<SubEstimacion?> Handle(GetSubEstimacionByIdQuery query);
        Task<List<SubEstimacion>> Handle(GetSubEstimacionesByEstimacionIdQuery query);
    }
}