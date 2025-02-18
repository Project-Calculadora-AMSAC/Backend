using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using System.Threading.Tasks;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;

public interface ICostoEstimadoRepository
{
    Task AddAsync(CostoEstimado costoEstimado);
    Task<CostoEstimado> GetByEstimacionId(int estimacionId);
}