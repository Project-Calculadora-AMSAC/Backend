using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IAtributoPamQueryService
{
    Task<AtributosPam> Handle(GetAtributoPamByIdQuery query);
    Task<List<AtributosPam>> Handle(GetAllAtributosPamQuery query);
    Task<List<AtributosPam>> Handle(GetAtributosPamByTipoPamIdQuery query);
}