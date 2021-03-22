using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class MyRepository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly Context _context;

        public MyRepository(Context context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();

            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            var entities = await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _context.Set<T>().Where(predicate.Compile()).AsQueryable().ToListAsync();
            return entities;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}