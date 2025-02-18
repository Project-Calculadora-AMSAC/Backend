using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;

namespace ProjectCalculadoraAMSAC.Infrastructure.Persistence.Repositories;

public class CostoEstimadoRepository : ICostoEstimadoRepository
{
    private readonly AppDbContext _context;

    public CostoEstimadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CostoEstimado costoEstimado)
    {
        await _context.CostoEstimados.AddAsync(costoEstimado);
    }

    public async Task<CostoEstimado> GetByEstimacionId(int estimacionId)
    {
        return await _context.CostoEstimados
            .FirstOrDefaultAsync(c => c.EstimacionId == estimacionId);
    }
}