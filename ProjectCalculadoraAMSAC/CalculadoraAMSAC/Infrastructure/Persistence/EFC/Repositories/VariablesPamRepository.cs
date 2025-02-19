using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class VariablesPamRepository : BaseRepository<VariablesPam>, IVariablesPamRepository
{
    private readonly AppDbContext _context;

    public VariablesPamRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<VariablesPam> GetByIdAsync(int id)
    {
        return await _context.Set<VariablesPam>()
            .Include(v => v.TipoPam)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<VariablesPam>> GetAllAsync()
    {
        return await _context.Set<VariablesPam>()
            .Include(v => v.TipoPam)
            .ToListAsync();
    }
}