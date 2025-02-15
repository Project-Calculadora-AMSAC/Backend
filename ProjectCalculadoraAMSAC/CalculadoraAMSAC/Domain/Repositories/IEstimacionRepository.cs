using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;


    public interface IEstimacionRepository : IBaseRepository<Estimacion>
    {
        Task<Estimacion> GetByIdAsync(int id);
        Task<List<Estimacion>> GetAllAsync();
    }
