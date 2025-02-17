using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface IProyectoRepository : IBaseRepository<Proyecto>
{
    Task<Proyecto> GetByIdAsync(int id);
    Task<List<Proyecto>> GetAllAsync();
}