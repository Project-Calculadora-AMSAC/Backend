using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class AtributoPamRepository : BaseRepository<AtributosPam>, IAtributoPamRepository
{
    private readonly AppDbContext _context;

    public AtributoPamRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<AtributosPam> GetByIdAsync(int id)
    {
        return await _context.AtributoPam
            .Include(a => a.TipoPam)
            .Include(a => a.UnidadDeMedida)
            .FirstOrDefaultAsync(a => a.AtributoPamId == id);
    }

    public async Task<List<AtributosPam>> GetAllAsync()
    {
        return await _context.AtributoPam
            .Include(a => a.TipoPam)
            .Include(a => a.UnidadDeMedida)
            .ToListAsync();
    }
    
    public async Task<List<AtributosPam>> GetAllByTipoPamAsync(int tipoPamId)
    {
        return await _context.AtributoPam
            .Where(a => a.TipoPamId == tipoPamId) 
            .Include(a => a.TipoPam)
            .Include(a => a.UnidadDeMedida)
            .ToListAsync();
    }
}