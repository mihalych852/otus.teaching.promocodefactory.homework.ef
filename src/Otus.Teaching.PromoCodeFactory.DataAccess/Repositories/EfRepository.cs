using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories; 

public class EfRepository<TEntity, TSource> : IRepository<TEntity> where TEntity : BaseEntity where TSource : DbContext
{
    private readonly TSource _databaseContext;
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(TSource databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = _databaseContext.Set<TEntity>();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TEntity>>(_dbSet.AsNoTracking().ToList());
    }

    public Task<TEntity> GetByIdAsync(Guid id)
    {
        return Task.FromResult<TEntity>(_dbSet.AsNoTracking().FirstOrDefault(x=>x.Id == id ));
    }

    public Task Add(TEntity entity)
    {
        _dbSet.Add(entity);
        return Task.CompletedTask;
    }

    public Task AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        return Task.CompletedTask;
    }

    public Task Update(TEntity entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRange(IEnumerable<Guid> entities)
    {
        _dbSet.RemoveRange((IEnumerable<TEntity>) entities.Select(GetByIdAsync));
        return Task.CompletedTask;
    }

    public Task DeleteByIdAsync(Guid id)
    {
        _dbSet.Remove(GetByIdAsync(id).Result);
        return Task.CompletedTask;
    }
}