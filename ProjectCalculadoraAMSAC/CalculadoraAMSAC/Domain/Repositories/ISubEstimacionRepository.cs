using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;


namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories
{
    public interface ISubEstimacionRepository
    {
        Task AddAsync(SubEstimacion subEstimacion);
        Task<SubEstimacion> GetByIdAsync(int id);
        Task<List<SubEstimacion>> GetByEstimacionIdAsync(int estimacionId);
    }
}