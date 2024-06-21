using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        
        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveEntitiesAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            return Data.AsQueryable();
        }

        public IQueryable<T> GetById(Guid id)
        {
            return Data.Where(x => x.Id == id).AsQueryable();
        }

        Task IRepository<T>.AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}