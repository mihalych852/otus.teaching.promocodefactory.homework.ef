using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IQueryable<T>> GetAllAsync();
        
        Task<T> GetByIdAsync(Guid id);

        void Update(T entity);

        void Add(T entity);

        void Remove(T entity);
    }
}