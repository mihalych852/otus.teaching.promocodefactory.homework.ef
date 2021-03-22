using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected List<T> Data { get; set; }

        public InMemoryRepository(List<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<T>> FilterByIdsAsync(List<Guid> ids)
        {
            return Task.FromResult(Data.Where(x => ids.Contains(x.Id)).AsEnumerable());
        }

        public Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(Data.Where(predicate.Compile()));
        }

        public Task AddAsync(T ent)
        {
            if (ent is null)
                throw new ApplicationException("Missing");

            Data.Add(ent);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(T ent)
        {
            if (ent is null)
                throw new ApplicationException("Not found");

            // delete
            Data.Remove(ent);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(T ent)
        {
            if (ent is null)
                throw new ApplicationException("Missing");

            Data = Data.Where(x => x.Id != ent.Id)
                .Append(ent)
                .ToList();

            return Task.CompletedTask;
        }
    }
}