using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<TEntity>
        where TEntity: BaseEntity
    {
        IQueryable<TEntity> GetAll();
        
        IQueryable<TEntity> GetById(Guid id);

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task SaveEntitiesAsync();
    }
}