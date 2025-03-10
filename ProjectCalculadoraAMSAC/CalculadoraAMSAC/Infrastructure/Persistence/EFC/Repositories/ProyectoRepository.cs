﻿using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Infrastructure.Persistence.EFC.Repositories;

public class ProyectoRepository : BaseRepository<Proyecto>, IProyectoRepository
    {
        private readonly AppDbContext _context;

        public ProyectoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Proyecto> GetByIdAsync(int id)
        {
            return await _context.Proyecto
                .Include(p => p.Estimaciones) 
                .FirstOrDefaultAsync(p => p.ProyectoId == id);
        }

        public async Task<List<Proyecto>> GetAllAsync()
        {
            return await _context.Proyecto
                .Include(p => p.Estimaciones) 
                .ToListAsync();
        }
    }
