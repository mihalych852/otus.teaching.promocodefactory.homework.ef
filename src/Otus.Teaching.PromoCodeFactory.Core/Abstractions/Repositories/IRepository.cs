using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetByIdAsync(Guid[] ids);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateAsync(T[] entities);
        Task<bool> DeleteAsync(Guid id);
        Task<T> GetEntityWithLoadedSpecificNavigationProperty(string propertyName, Guid id);
    }
}