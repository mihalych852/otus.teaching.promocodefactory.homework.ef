using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly DataContext DataContext;

        protected EfRepository(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DataContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await DataContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(T entity)
        {
            await DataContext.Set<T>().AddAsync(entity);
            await DataContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DataContext.Set<T>().AddRangeAsync(entities);
            await DataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            DataContext.Set<T>().Update(entity);
            await DataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await DataContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            DataContext.Set<T>().Remove(entity);
            await DataContext.SaveChangesAsync();
        }
    }
}