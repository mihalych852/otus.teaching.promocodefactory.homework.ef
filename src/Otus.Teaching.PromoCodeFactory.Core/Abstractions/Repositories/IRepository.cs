using Otus.Teaching.PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id, CancellationToken cancel = default);
        Task<IEnumerable<T>> GetByIdAsync(Guid[] ids, CancellationToken cancel = default);
        Task<T> CreateAsync(T entity, CancellationToken cancel = default);
        Task UpdateAsync(T entity, CancellationToken cancel = default);
        Task UpdateAsync(T[] entities, CancellationToken cancel = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancel = default);
        Task<T> GetEntityWithLoadedSpecificNavigationProperty(string propertyName, Guid id, CancellationToken cancel = default);
    }
}