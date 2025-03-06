using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class EstimacionRepository : BaseRepository<Estimacion>, IEstimacionRepository
{
    private readonly AppDbContext _context;

    public EstimacionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Estimacion> GetByIdAsync(int id)
    {
        return await _context.Estimaciones
            .Include(e => e.Proyecto)
            .Include(e => e.TipoPam)
            .ThenInclude(tp => tp.Variables) 
            .Include(e => e.Valores)
            .ThenInclude(v => v.AtributoPam)
            .FirstOrDefaultAsync(e => e.EstimacionId == id);
    }


    public async Task<List<Estimacion>> GetAllAsync()
    {
        return await _context.Estimaciones
            .Include(e => e.Proyecto)
            .Include(e => e.TipoPam)
            .ThenInclude(tp => tp.Variables) 
            .Include(e => e.Valores)
            .ThenInclude(v => v.AtributoPam)
            .ToListAsync();
    }
    
    public IQueryable<Estimacion> GetQueryable()
    {
        return _context.Estimaciones.Include(e => e.CostoEstimado);
    }
    public async Task<List<Estimacion>> GetByProyectoIdAsync(int proyectoId)
    {
        return await _context.Estimaciones
            .Where(e => e.ProyectoId == proyectoId)
            .ToListAsync();
    }

    public async Task<List<Estimacion>> GetByTipoPamIdAsync(int tipoPamId)
    {
        return await _context.Estimaciones
            .Where(e => e.TipoPamId == tipoPamId)
            .ToListAsync();
    }
    public async Task<List<Estimacion>> GetByProyectoIdAndTipoPamIdAsync(int? proyectoId, int? tipoPamId)
    {
        var query = _context.Estimaciones.AsQueryable();

        if (proyectoId.HasValue)
            query = query.Where(e => e.ProyectoId == proyectoId.Value);

        if (tipoPamId.HasValue)
            query = query.Where(e => e.TipoPamId == tipoPamId.Value);

        return await query
            .Include(e => e.CostoEstimado)
            .Include(e => e.Proyecto)
            .Include(e => e.TipoPam)
            .Include(e => e.Valores)
            .ToListAsync();
    }
    
    public async Task<string?> GetUltimoCodPamAsync()
    {
        return await _context.Estimaciones
            .Where(e => e.CodPam.StartsWith("SN-"))
            .OrderByDescending(e => e.CodPam)
            .Select(e => e.CodPam)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByCodPamAsync(string codPam)
    {
        return await _context.Estimaciones.AnyAsync(e => e.CodPam == codPam);
    }


}
