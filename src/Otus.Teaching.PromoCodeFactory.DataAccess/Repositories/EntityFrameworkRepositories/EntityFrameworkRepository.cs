using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories.EntityFrameworkRepositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly PromoCodeDataContext DbContext;

        public EntityFrameworkRepository(PromoCodeDataContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public virtual Task<T> GetByIdAsync(Guid id)
        {
            return DbContext.Set<T>().FindAsync(id).AsTask();
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}