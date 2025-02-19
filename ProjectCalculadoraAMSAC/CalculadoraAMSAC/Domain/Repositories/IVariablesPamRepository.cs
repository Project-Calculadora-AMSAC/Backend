using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface IVariablesPamRepository : IBaseRepository<VariablesPam>
{
    Task<VariablesPam> GetByIdAsync(int id);
    Task<List<VariablesPam>> GetAllAsync();
}