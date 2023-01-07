using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PromocodeFactoryDb _db;
        private readonly ILogger<T> _logger;
        private string _typeName;
        private string TypeName => _typeName ??= typeof(T).Name;
        public EfRepository(PromocodeFactoryDb db, ILogger<T> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation($"Getting all entities of {TypeName}");
            return Task.FromResult<IEnumerable<T>>(_db.Set<T>());
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting Entity of {TypeName} by Id ...");
            var result = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            _logger.LogWarning(result is null ? "Failed, value is NULL" : "Success");
            return result;
        }

        public async Task<IEnumerable<T>> GetByIdAsync(Guid[] ids)
        {
            _logger.LogInformation($"Getting array of Entity of {TypeName} by Ids ...");
            _logger.LogInformation($"Checking for null...");
            if (ids is null) throw new ArgumentNullException(nameof(ids));
            _logger.LogInformation($"Checking for count of entities above null ...");
            if (ids.Length < 1) throw new ArgumentException(nameof(ids));

            var result = await _db.Set<T>()
                .Where(e => ids.Contains(e.Id))
                .ToArrayAsync();
            _logger.LogInformation($"Success, ids got {ids.Length}, entities send {result.Length} ");
            return result;
        }

        public async Task<T> CreateAsync(T entity)
        {
            _logger.LogInformation($"Creating entity of {TypeName} ...");
            _logger.LogInformation($"Checking for null ...");
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _logger.LogInformation($"Adding entity into DbContext ...");
            await _db.Set<T>().AddAsync(entity);
            _logger.LogInformation($"Saving changes ...");
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Success, Id {entity.Id}");
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _logger.LogInformation($"Creating entity of {TypeName} ...");
            var oldEntity =  await GetByIdAsync(entity.Id);
            _logger.LogInformation($"Checking for null ...");
            if (oldEntity is null) throw new ArgumentException(nameof(entity));
            _logger.LogInformation($"Setting the value ...");
            _db.Entry(oldEntity).CurrentValues.SetValues(entity);
            _logger.LogInformation($"Save changes ...");
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Success");
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Deleting of entity ...");
            var entity = await GetByIdAsync(id);
            _logger.LogInformation($"Checking for existing ...");
            if (entity is null) throw new ArgumentException(nameof(id));
            _db.Set<T>().Remove(entity);
            _logger.LogInformation($"Save changes ...");
            await _db.SaveChangesAsync();
            var result = await GetByIdAsync(id) is null;
            if (result)
                _logger.LogInformation("Success");
            else
                _logger.LogWarning("Failed");
            return result;
        }
    }
}
