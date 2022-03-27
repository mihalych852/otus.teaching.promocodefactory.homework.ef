using System;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;


namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IEfRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(Guid id);
    }
}
