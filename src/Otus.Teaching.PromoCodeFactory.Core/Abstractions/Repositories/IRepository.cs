using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(params string[] includedPropertyNames);
        
        Task<T> GetByIdAsync(Guid id, params string[] includedPropertyNames);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] includedPropertyNames);
        
        Task<T> CreateAsync(T record);

        Task<T> UpdateAsync(T record);

        Task<T> DeleteByIdAsync(Guid id);
    }
}