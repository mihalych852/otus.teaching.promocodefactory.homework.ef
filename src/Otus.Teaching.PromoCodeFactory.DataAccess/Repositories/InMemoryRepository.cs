using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories {
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data) {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync() {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id) {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task Add(T entity) =>
            AddRange(new[] { entity });

        public Task AddRange(IEnumerable<T> entities) {
            var newRecord = Data.ToList();
            newRecord.AddRange(entities);
            Data = newRecord;
            return Task.CompletedTask;
        }

        public Task Update(T entity) {
            var newRecord = Data.Where(x => x.Id != entity.Id).ToList();
            newRecord.Add(entity);
            Data = newRecord;
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<Guid> records) {
            Data = Data.Where(x => !records.Contains(x.Id));
            return Task.CompletedTask;
        }

        public Task DeleteByIdAsync(Guid id) {
            Data = Data.Where(x => x.Id != id);
            return Task.CompletedTask;
        }

        public Task<T> GetAsync(Guid id)
            => Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
    }
}