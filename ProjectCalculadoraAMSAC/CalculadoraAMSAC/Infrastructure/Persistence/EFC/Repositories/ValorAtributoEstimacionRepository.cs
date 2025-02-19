﻿namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class ValorAtributoEstimacionRepository : BaseRepository<ValorAtributoEstimacion>, IValorAtributoEstimacionRepository
{
    private readonly AppDbContext _context;

    public ValorAtributoEstimacionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ValorAtributoEstimacion> GetByIdAsync(int id)
    {
        return await _context.ValoresAtributosEstimacion
            .Include(v => v.Estimacion)
            .Include(v => v.AtributoPam)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<ValorAtributoEstimacion>> GetAllByEstimacionIdAsync(int estimacionId)
    {
        return await _context.ValoresAtributosEstimacion
            .Where(v => v.EstimacionId == estimacionId)
            .Include(v => v.AtributoPam)
            .ToListAsync();
    }
}