using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IVariablesPamQueryService
{
    Task<VariablesPam> Handle(GetVariablesPamByIdQuery query);
    Task<List<VariablesPam>> Handle(GetAllVariablesPamQuery query);
}