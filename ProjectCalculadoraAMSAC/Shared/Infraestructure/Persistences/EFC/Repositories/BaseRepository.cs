﻿using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;
using ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Configuration;

namespace ProjectCalculadoraAMSAC.Shared.Infraestructure.Persistences.EFC.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }
    public async Task AddSync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<TEntity?> FindByIdAsync(Guid id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
        
    }
 
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }
    
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await FindByIdAsync(id);
        if (entity == null) return;

        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
    }
    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }
}