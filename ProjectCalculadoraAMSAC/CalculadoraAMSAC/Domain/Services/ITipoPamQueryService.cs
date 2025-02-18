using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;


public interface ITipoPamQueryService
{
    Task<List<TipoPam>> Handle(GetAllTipoPamQuery query);
    Task<TipoPam?> Handle(GetTipoPamByIdQuery query);
}