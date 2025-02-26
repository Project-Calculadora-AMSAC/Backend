using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class ValorAtributoEstimacionRepository : BaseRepository<ValorAtributoEstimacion>, IValorAtributoEstimacionRepository
{
    private readonly AppDbContext _context;

    public ValorAtributoEstimacionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ValorAtributoEstimacion?> GetByIdAsync(int id)
    {
        return await _context.ValoresAtributosEstimacion
            .Include(v => v.SubEstimacion)
            .Include(v => v.AtributoPam)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<ValorAtributoEstimacion>> GetAllBySubEstimacionIdAsync(int subEstimacionId)
    {
        return await _context.ValoresAtributosEstimacion
            .Where(v => v.SubEstimacionId == subEstimacionId)
            .Include(v => v.AtributoPam)
            .ToListAsync();
    }
}