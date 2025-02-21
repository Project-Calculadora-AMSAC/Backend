using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class EstimacionQueryService(IEstimacionRepository estimacionRepository) : IEstimacionQueryService
{
 
    // ✅ Obtener una única estimación por ID, incluyendo `CostoEstimado`, `Proyecto`, `TipoPam` y `Valores`
    public async Task<Estimacion?> Handle(GetEstimacionByIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.CostoEstimado)         // Incluir CostoEstimado
            .Include(e => e.Proyecto)              // Incluir Proyecto
            .Include(e => e.TipoPam)               // Incluir TipoPam
            .Include(e => e.Valores)               // Incluir Valores
            .FirstOrDefaultAsync(e => e.EstimacionId == query.EstimacionId);
    }

    // ✅ Obtener todas las estimaciones incluyendo `CostoEstimado`, `Proyecto`, `TipoPam` y `Valores`
    public async Task<IEnumerable<Estimacion>> Handle(GetAllEstimacionesQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.CostoEstimado)         // Incluir CostoEstimado
            .Include(e => e.Proyecto)              // Incluir Proyecto
            .Include(e => e.TipoPam)               // Incluir TipoPam
            .Include(e => e.Valores)               // Incluir Valores
            .ToListAsync();
    }
    
    // ✅ Obtener el costo total de todas las estimaciones de un proyecto
    public async Task<decimal?> Handle(GetTotalCostByProjectIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Where(e => e.ProyectoId == query.ProyectoId) // 🔹 Extraer `query.ProyectoId`
            .SumAsync(e => e.CostoEstimado != null ? e.CostoEstimado.TotalEstimado : 0);
    }
    public async Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable() // IQueryable<Estimacion>
            .Include(e => e.CostoEstimado)      // Incluir CostoEstimado
            .Include(e => e.Proyecto)           // Incluir Proyecto
            .Include(e => e.TipoPam)            // Incluir TipoPam
            .Include(e => e.Valores)            // Incluir Valores
            .Where(e => e.ProyectoId == query.ProyectoId) // Filtrar por ProyectoId
            .ToListAsync(); // Ejecutar la consulta de forma asincrónica
    }
    public async Task<List<Estimacion>> Handle(GetEstimacionesByTipoPamIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable() // IQueryable<Estimacion>
            .Include(e => e.CostoEstimado)      // Incluir CostoEstimado
            .Include(e => e.Proyecto)           // Incluir Proyecto
            .Include(e => e.TipoPam)            // Incluir TipoPam
            .Include(e => e.Valores)            // Incluir Valores
            .Where(e => e.TipoPamId == query.TipoPamId) // Filtrar por ProyectoId
            .ToListAsync(); // Ejecutar la consulta de forma asincrónica
                            }
    public async Task<List<Estimacion>> Handle(GetEstimacionesByProyectoIdAndTipoPamIdQuery query)
    {
        return await estimacionRepository
            .GetQueryable()
            .Include(e => e.CostoEstimado)
            .Include(e => e.Proyecto)
            .Include(e => e.TipoPam)
            .Include(e => e.Valores)
            .Where(e => e.TipoPamId == query.TipoPamId && e.ProyectoId == query.ProyectoId) // ✅ Corrección aquí
            .ToListAsync();
    }
}