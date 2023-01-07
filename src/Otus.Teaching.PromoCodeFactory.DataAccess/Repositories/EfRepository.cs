using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PromocodeFactoryDb _db;

        public EfRepository(PromocodeFactoryDb db)
        {
            _db = db;
        }

        public Task<IEnumerable<T>> GetAllAsync()
            {
            return Task.FromResult<IEnumerable<T>>(_db.Set<T>());
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            return result;
        }

        public async Task<IEnumerable<T>> GetByIdAsync(Guid[] ids)
        {
            if (ids is null) throw new ArgumentNullException(nameof(ids));
            if (ids.Length < 1) throw new ArgumentException(nameof(ids));

            var result = await _db.Set<T>()
                .Where(e => ids.Contains(e.Id))
                .ToArrayAsync();

            return result;
        }

        public async Task<T> CreateAsync(T entity)
        {
            if(entity is null) throw new ArgumentNullException(nameof(entity));
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            var oldEntity =  await GetByIdAsync(entity.Id);

            _db.Entry(oldEntity).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
            var result = await GetByIdAsync(id) is null;
            return result;
        }
    }
}
