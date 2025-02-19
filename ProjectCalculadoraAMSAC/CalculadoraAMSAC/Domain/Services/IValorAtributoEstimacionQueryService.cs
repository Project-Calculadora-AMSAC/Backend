using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IValorAtributoEstimacionQueryService
{
    Task<IEnumerable<ValorAtributoEstimacion>> Handle(GetAllValoresAtributoEstimacionQuery query);
    Task<ValorAtributoEstimacion?> Handle(GetValorAtributoEstimacionByIdQuery query);
}