using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _db;
        public EfRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_db.Set<T>().AsSplitQuery());
        }
        /// <inheritdoc />
        public async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            return result;
        }
        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        /// <inheritdoc />
        public async Task UpdateAsync(T entity)
        {
            var oldEntity = await GetByIdAsync(entity.Id);

            if (oldEntity is null)
                throw new ArgumentException(nameof(entity));

            _db.Entry(oldEntity).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
        }
        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null) 
                throw new ArgumentException(nameof(id));

            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
            var result = await GetByIdAsync(id) is null;
            return result;
        }

        /// <inheritdoc />
        public async Task<T> GetEntityWithLoadedSpecificNavigationProperty(string propertyName, Guid id)
        {
            var ent = await GetByIdAsync(id);

            if (ent is null) 
                throw new ArgumentException(nameof(id));

            if (_db?.Set<T>().Entry(ent).Navigation(propertyName) is null)
                throw new NullReferenceException(propertyName);

            if (_db.Set<T>().Entry(ent).Navigation(propertyName).IsLoaded)
            {
                return ent;
            }

            await _db.Set<T>().Entry(ent).Navigation(propertyName).LoadAsync();
            return ent;
        }

    }
}
