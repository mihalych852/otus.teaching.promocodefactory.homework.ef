

using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;


namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly EFDataContext _dataContext;

        public EfRepository(EFDataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dataContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
            if (entity == null) return false;
            _dataContext.Set<T>().Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();

        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dataContext.Set<T>().SingleAsync(e => e.Id == id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            return await _dataContext.SaveChangesAsync() > 0;

        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
