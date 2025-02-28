using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;


    public interface IEstimacionRepository : IBaseRepository<Estimacion>
    {
        Task<Estimacion> GetByIdAsync(int id);
        Task<List<Estimacion>> GetAllAsync();
        IQueryable<Estimacion> GetQueryable(); // ✅ Permite usar `Include()`
        Task<List<Estimacion>> GetByProyectoIdAsync(int proyectoId);
        Task<List<Estimacion>> GetByTipoPamIdAsync(int tipoPamId);
        Task<List<Estimacion>> GetByProyectoIdAndTipoPamIdAsync(int? proyectoId, int? tipoPamId);
        Task<string?> GetUltimoCodPamAsync();
        Task<bool> ExistsByCodPamAsync(string codPam);


    }
