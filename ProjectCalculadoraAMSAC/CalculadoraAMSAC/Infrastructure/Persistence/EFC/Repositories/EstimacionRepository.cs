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
            .Include(e => e.SubEstimaciones) 
            .ThenInclude(se => se.TipoPam) // ✅ Asegura que `TipoPam` se cargue correctamente
                
            .Include(e => e.SubEstimaciones) 
            .ThenInclude(se => se.Valores) // ✅ Asegura que `Valores` también se carguen
            .FirstOrDefaultAsync(e => e.EstimacionId == id);
    }

    public async Task<List<Estimacion>> GetAllAsync()
    {
        return await _context.Estimaciones
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .ToListAsync();
    }

    public IQueryable<Estimacion> GetQueryable()
    {
        return _context.Estimaciones
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores);
    }

    public async Task<List<Estimacion>> GetByProyectoIdAsync(int proyectoId)
    {
        return await _context.Estimaciones
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .Where(e => e.ProyectoId == proyectoId)
            .ToListAsync();
    }

    public async Task<List<Estimacion>> GetBySubEstimacionTipoPamIdAsync(int tipoPamId)
    {
        return await _context.Estimaciones
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .Where(e => e.SubEstimaciones.Any(se => se.TipoPamId == tipoPamId))
            .ToListAsync();
    }

    public async Task<List<Estimacion>> GetByProyectoIdAndSubEstimacionTipoPamIdAsync(int? proyectoId, int? tipoPamId)
    {
        var query = _context.Estimaciones
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .AsQueryable();

        if (proyectoId.HasValue)
            query = query.Where(e => e.ProyectoId == proyectoId.Value);

        if (tipoPamId.HasValue)
            query = query.Where(e => e.SubEstimaciones.Any(se => se.TipoPamId == tipoPamId.Value));

        return await query.ToListAsync();
    }
    public async Task<Estimacion> GetByIdWithSubEstimacionesAsync(int estimacionId)
    {
        var estimacion = await _context.Estimaciones
            .Include(e => e.SubEstimaciones) // 🔹 Asegurar que se incluyen
            .ThenInclude(se => se.TipoPam) // 🔹 Cargar TipoPam de cada SubEstimacion
            .FirstOrDefaultAsync(e => e.EstimacionId == estimacionId);

        if (estimacion == null)
            Console.WriteLine($"ERROR: No se encontró la estimación con ID {estimacionId}.");

        if (estimacion.SubEstimaciones == null || !estimacion.SubEstimaciones.Any())
            Console.WriteLine($"ERROR: La estimación {estimacionId} no tiene subestimaciones cargadas.");

        return estimacion;
    }



}