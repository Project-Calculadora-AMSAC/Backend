using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IEstimacionQueryService
{
    Task<Estimacion?> Handle(GetEstimacionByIdQuery query);
    Task<IEnumerable<Estimacion>> Handle(GetAllEstimacionesQuery query);
    Task<decimal?> Handle(GetTotalCostByProjectIdQuery query); // 🔹 Ahora recibe un objeto `query`
}