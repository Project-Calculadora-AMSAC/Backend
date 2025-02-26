using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface IValorAtributoEstimacionRepository : IBaseRepository<ValorAtributoEstimacion>
{
    Task<ValorAtributoEstimacion?> GetByIdAsync(int id);
    Task<List<ValorAtributoEstimacion>> GetAllBySubEstimacionIdAsync(int subEstimacionId);
}