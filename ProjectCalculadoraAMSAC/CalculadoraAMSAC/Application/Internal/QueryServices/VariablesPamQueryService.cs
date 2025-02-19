using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class VariablesPamQueryService : IVariablesPamQueryService
{
    private readonly IVariablesPamRepository _variablesPamRepository;

    public VariablesPamQueryService(IVariablesPamRepository variablesPamRepository)
    {
        _variablesPamRepository = variablesPamRepository;
    }

    public async Task<VariablesPam> Handle(GetVariablesPamByIdQuery query)
    {
        return await _variablesPamRepository.GetByIdAsync(query.Id);
    }

    public async Task<List<VariablesPam>> Handle(GetAllVariablesPamQuery query)
    {
        return await _variablesPamRepository.GetAllAsync();
    }
}