using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> 
        : IRepository<T>
        where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<T> _entitySet;

        public EfRepository(DbContext context)
        {
            _dbContext = context;
            _entitySet = _dbContext.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entitySet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetListByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _entitySet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _entitySet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _entitySet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _entitySet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _entitySet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}

