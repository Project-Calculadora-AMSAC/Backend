using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface IAtributoPamRepository : IBaseRepository<AtributosPam>
{
    Task<AtributosPam> GetByIdAsync(int id);
    Task<List<AtributosPam>> GetAllAsync();
}