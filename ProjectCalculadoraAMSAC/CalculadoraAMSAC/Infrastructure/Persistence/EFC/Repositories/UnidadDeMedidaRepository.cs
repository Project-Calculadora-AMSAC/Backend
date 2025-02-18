using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class UnidadDeMedidaRepository : BaseRepository<UnidadDeMedida>, IUnidadDeMedidaRepository
{
    private readonly AppDbContext _context;

    public UnidadDeMedidaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UnidadDeMedida> GetByIdAsync(int id)
    {
        return await _context.UnidadesDeMedida.FindAsync(id);
    }

    public async Task<List<UnidadDeMedida>> GetAllAsync()
    {
        return await _context.UnidadesDeMedida.ToListAsync();
    }
}