using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories
{
    public class SubEstimacionRepository : ISubEstimacionRepository
    {
        private readonly AppDbContext _context;

        public SubEstimacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SubEstimacion subEstimacion)
        {
            await _context.SubEstimaciones.AddAsync(subEstimacion);
        }

        public async Task<SubEstimacion> GetByIdAsync(int id)
        {
            return await _context.SubEstimaciones
                .Include(se => se.TipoPam)
                .Include(se => se.Valores)
                .FirstOrDefaultAsync(se => se.Id == id);
        }

        public async Task<List<SubEstimacion>> GetByEstimacionIdAsync(int estimacionId)
        {
            return await _context.SubEstimaciones
                .Include(se => se.TipoPam)
                .Include(se => se.Valores)
                .Include(se => se.CostoEstimado)
                .Where(se => se.Estimacion.EstimacionId == estimacionId)
                .ToListAsync();
        }
    }
}