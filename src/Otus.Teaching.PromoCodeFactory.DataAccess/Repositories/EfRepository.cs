using Microsoft.EntityFrameworkCore;
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
        where T : BaseEntity
    {
        protected readonly DbContext Context;
        private readonly DbSet<T> _entitySet;
        protected IEnumerable<T> Data { get; set; }

        protected EfRepository(DbContext context, IEnumerable<T> data)
        {
            Context = context;
            _entitySet = Context.Set<T>();
            Data = data;
        }

        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _entitySet.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetListByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await _entitySet.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity)
        {
            await _entitySet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _entitySet.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _entitySet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _entitySet.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}

