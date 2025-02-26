using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IEstimacionQueryService
{
    Task<Estimacion?> Handle(GetEstimacionByIdQuery query);
    Task<IEnumerable<Estimacion>> Handle(GetAllEstimacionesQuery query);
    Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdQuery query);
    Task<List<Estimacion>> Handle(GetEstimacionesByTipoPamIdQuery query);
    Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdAndTipoPamIdQuery query);
    public Task<Estimacion> GetByIdWithSubEstimacionesAsync(int estimacionId);

}