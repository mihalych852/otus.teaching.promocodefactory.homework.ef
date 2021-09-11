using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PromoCodeDataContext _dbContext;

        public EntityFrameworkRepository(PromoCodeDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var allItems = await _dbContext.Set<T>().ToListAsync().ConfigureAwait(false);
            return allItems.AsEnumerable();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var item = await _dbContext.Set<T>().FindAsync(id).ConfigureAwait(false);
            return item;
        }
    }
}