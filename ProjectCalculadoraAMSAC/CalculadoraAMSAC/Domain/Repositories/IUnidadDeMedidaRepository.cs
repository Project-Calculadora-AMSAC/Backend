using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface IUnidadDeMedidaRepository: IBaseRepository<UnidadDeMedida>
{
    Task<UnidadDeMedida> GetByIdAsync(int id);
    Task<List<UnidadDeMedida>> GetAllAsync();
}