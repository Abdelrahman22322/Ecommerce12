﻿using System.Linq.Expressions;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
     private readonly DbSet<TEntity> _entities;

    public GenericRepository(ApplicationDbContext context, DbSet<TEntity> entities)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }


    public async Task AddAsync(TEntity entity)
    {
         await _entities.AddAsync(entity);
    }

    public async Task<TEntity> AddRangeAsync(TEntity entities)
    {

        await _entities.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }


    public async Task<TEntity> UpdateAsync(TEntity entity)
    {

        _entities.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {

        _entities.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
        
    }

    public async Task<TEntity> DeleteRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return entities.FirstOrDefault();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {

        return await _entities.FindAsync(id);
       
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, string? includeword)
    {

        if (predicate != null)
        {
            _entities.Where(predicate);
        }

        if (includeword is not null)
        {
            foreach (var item in includeword.Split(new char[]{','} , StringSplitOptions.RemoveEmptyEntries ))
            {
                _entities.Include(item);
            }
        }

       
        return await _entities.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string? includeword)
    {

        if (predicate != null)
        {
            _entities.Where(predicate);
        }

        if (includeword is not null)
        {
            foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _entities.Include(item);
            }
        }

       

        return await _entities.ToListAsync();
    }

    public async Task SaveAsync()
    {

        await _context.SaveChangesAsync();
    }
}