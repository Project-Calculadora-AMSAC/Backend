using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IUnidadDeMedidaQueryService
{
    Task<UnidadDeMedida> Handle(GetUnidadDeMedidaByIdQuery query);
    Task<List<UnidadDeMedida>> Handle(GetAllUnidadesDeMedidaQuery query);
}