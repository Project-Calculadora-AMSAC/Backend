using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface ITipoPamRepository : IBaseRepository<TipoPam>
{
    Task<TipoPam?> GetByIdAsync(int id);
    Task<List<TipoPam>> GetAllAsync();
    Task<TipoPam?> GetByIdWithVariablesAsync(int id); // ✅ Nuevo método
}