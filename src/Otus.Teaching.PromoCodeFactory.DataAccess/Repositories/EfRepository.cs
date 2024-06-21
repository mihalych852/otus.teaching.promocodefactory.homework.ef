using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    internal class EfRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : BaseEntity where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public EfRepository(TDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetById(Guid id)
        {
            return _context.Set<TEntity>().Where(x => x.Id == id).AsQueryable();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {            
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task SaveEntitiesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
