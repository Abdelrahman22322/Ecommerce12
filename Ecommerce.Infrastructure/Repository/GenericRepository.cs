using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity?> _entities;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity? entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task<TEntity> AddRangeAsync(TEntity entities)
    {
        throw new NotImplementedException();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity?> entities)
    {
        await _entities.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity?> UpdateAsync(TEntity? entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> DeleteAsync(TEntity? entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> DeleteRange(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity?> entities)
    {
        _entities.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync(Expression<Func<TEntity?, bool>?>? predicate = null, string? includeword = null)
    {
        IQueryable<TEntity?> query = _entities.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeword is not null)
        {
            foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity?>> FindAsync(Expression<Func<TEntity?, bool>> predicate, string? includeword)
    {
        IQueryable<TEntity?> query = _entities.AsNoTracking();

        if (includeword is not null)
        {
            foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> FindAsync1(Expression<Func<TEntity?, bool>> predicate, string? includeword)
    {
        IQueryable<TEntity?> query = _entities.AsNoTracking();

        if (includeword is not null)
        {
            foreach (var item in includeword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> FindCompositeKeyAsync(Expression<Func<TEntity?, bool>> predicate)
    {
        return await _entities.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    // New Attach method
    public void Attach(TEntity entity)
    {
        _entities.Attach(entity);
    }
}
