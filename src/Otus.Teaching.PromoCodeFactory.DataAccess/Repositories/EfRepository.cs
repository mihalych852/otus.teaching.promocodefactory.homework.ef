using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation($"Getting all entities of {TypeName}");
            return Task.FromResult<IEnumerable<T>>(_db.Set<T>().AsSplitQuery());
        }

        /// <summary>
        /// Return the entity by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Getting Entity of {TypeName} by Id ...");
            var result = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancel);
            _logger.LogWarning(result is null ? "Failed, value is NULL" : "Success");
            return result;
        }

        /// <summary>
        /// Return the entities get by array of IDs
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<T>> GetByIdAsync(Guid[] ids, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Getting array of Entity of {TypeName} by Ids ...");
            _logger.LogInformation($"Checking for null...");
            if (ids is null) throw new ArgumentNullException(nameof(ids));
            _logger.LogInformation($"Checking for count of entities above null ...");
            if (ids.Length < 1) throw new ArgumentException(nameof(ids));

            var result = await _db.Set<T>()
                .Where(e => ids.Contains(e.Id))
                .ToArrayAsync(cancel);
            _logger.LogInformation($"Success, ids got {ids.Length}, entities send {result.Length} ");
            return result;
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<T> CreateAsync(T entity, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Creating entity of {TypeName} ...");
            _logger.LogInformation($"Checking for null ...");
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _logger.LogInformation($"Adding entity into DbContext ...");
            await _db.Set<T>().AddAsync(entity, cancel);
            _logger.LogInformation($"Saving changes ...");
            await _db.SaveChangesAsync(cancel);
            _logger.LogInformation($"Success, Id {entity.Id}");
            return entity;
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task UpdateAsync(T entity, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Creating entity of {TypeName} ...");
            var oldEntity =  await GetByIdAsync(entity.Id, cancel);
            _logger.LogInformation($"Checking for null ...");
            if (oldEntity is null) throw new ArgumentException(nameof(entity));
            _logger.LogInformation($"Setting the value ...");
            _db.Entry(oldEntity).CurrentValues.SetValues(entity);
            _logger.LogInformation($"Save changes ...");
            await _db.SaveChangesAsync(cancel);
            _logger.LogInformation($"Success");
        }

        /// <summary>
        /// Update the array of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task UpdateAsync(T[] entities, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Updating array of entities of {TypeName} ...");
            _logger.LogInformation($"Checking for null ...");
            if (entities is null) throw new ArgumentException(nameof(entities));

            var listEntities = entities.ToList();

            for (int i = 0; i < entities.Length; i++)
            {
                var oldEntity = await GetByIdAsync(listEntities[i].Id, cancel);
                _logger.LogInformation($"Setting the value: number - {i + 1}; id - {listEntities[i].Id} ...");
                _db.Entry(oldEntity).CurrentValues.SetValues(listEntities[i]);
            }

            _logger.LogInformation($"Save changes ...");
            await _db.SaveChangesAsync(cancel);
            _logger.LogInformation($"Success");
        }

        /// <summary>
        /// Delete the entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Deleting of entity ...");
            var entity = await GetByIdAsync(id, cancel);
            _logger.LogInformation($"Checking for existing ...");
            if (entity is null) throw new ArgumentException(nameof(id));
            _db.Set<T>().Remove(entity);
            _logger.LogInformation($"Save changes ...");
            await _db.SaveChangesAsync(cancel);
            var result = await GetByIdAsync(id, cancel) is null;
            if (result)
                _logger.LogInformation("Success");
            else
                _logger.LogWarning("Failed");
            return result;
        }

        /// <summary>
        /// Load specific navigation property like include for Generic.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="id"></param>
        /// <param name="cancel"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public async Task<T> GetEntityWithLoadedSpecificNavigationProperty(string propertyName, Guid id, CancellationToken cancel = default)
        {
            _logger.LogInformation($"Loading navigation property '{propertyName}' for '{typeof(T)}' Id - {id} ...");
            var ent = await GetByIdAsync(id, cancel);
            _logger.LogInformation("Check entity for null ...");
            if (ent is null) throw new ArgumentException(nameof(id));
            _logger.LogInformation("Check navigation property of entity for null ...");
            if (_db?.Set<T>().Entry(ent).Navigation(propertyName) is null)
                throw new NullReferenceException(propertyName);
            if (_db.Set<T>().Entry(ent).Navigation(propertyName).IsLoaded)
            {
                _logger.LogInformation("Navigation property already is Loaded");
                return ent;
            }

            await _db.Set<T>().Entry(ent).Navigation(propertyName).LoadAsync(cancel);
            _logger.LogInformation("Success");
            return ent;
        }

    }
}
