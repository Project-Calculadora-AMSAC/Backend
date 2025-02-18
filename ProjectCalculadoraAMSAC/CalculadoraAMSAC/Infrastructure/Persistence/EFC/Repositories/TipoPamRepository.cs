using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;


public class TipoPamRepository : BaseRepository<TipoPam>, ITipoPamRepository
{
    private readonly AppDbContext _context;

    public TipoPamRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<TipoPam?> GetByIdAsync(int id)
    {
        return await _context.TipoPam
            .Include(tp => tp.Atributos)
            .Include(tp => tp.Variables)
            .FirstOrDefaultAsync(tp => tp.Id == id);
    }

    public async Task<List<TipoPam>> GetAllAsync()
    {
        return await _context.TipoPam
            .Include(tp => tp.Atributos)
            .Include(tp => tp.Variables)
            .ToListAsync();
    }

    public async Task<TipoPam?> GetByIdWithVariablesAsync(int id)
    {
        return await _context.TipoPam
            .Include(tp => tp.Variables) // ✅ Cargar las variables correctamente
            .FirstOrDefaultAsync(tp => tp.Id == id);
    }
}