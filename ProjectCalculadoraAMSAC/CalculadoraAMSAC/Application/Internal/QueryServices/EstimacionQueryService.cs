using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class EstimacionQueryService(IEstimacionRepository estimacionRepository) : IEstimacionQueryService
{
    public async Task<Estimacion?> Handle(GetEstimacionByIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones) // Incluir SubEstimaciones
                .ThenInclude(se => se.TipoPam) // Incluir TipoPam dentro de SubEstimaciones
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores) // Incluir Valores dentro de SubEstimaciones
            .FirstOrDefaultAsync(e => e.EstimacionId == query.EstimacionId);
    }

    public async Task<IEnumerable<Estimacion>> Handle(GetAllEstimacionesQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .ToListAsync();
    }
    


    public async Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .Where(e => e.ProyectoId == query.ProyectoId)
            .ToListAsync();
    }

    public async Task<List<Estimacion>> Handle(GetEstimacionesByTipoPamIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .Where(e => e.SubEstimaciones.Any(se => se.TipoPamId == query.TipoPamId))
            .ToListAsync();
    }

    public async Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdAndTipoPamIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.Proyecto)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.TipoPam)
            .Include(e => e.SubEstimaciones)
                .ThenInclude(se => se.Valores)
            .Where(e => e.ProyectoId == query.ProyectoId && e.SubEstimaciones.Any(se => se.TipoPamId == query.TipoPamId))
            .ToListAsync();
    }
    public async Task<Estimacion> GetByIdWithSubEstimacionesAsync(int estimacionId)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.SubEstimaciones)
            .ThenInclude(se => se.TipoPam) // Incluir el TipoPam de cada SubEstimacion
            .Include(e => e.SubEstimaciones)
            .ThenInclude(se => se.CostoEstimado) // Incluir el CostoEstimado de cada SubEstimacion (si existe)
            .FirstOrDefaultAsync(e => e.EstimacionId == estimacionId);
    }
}