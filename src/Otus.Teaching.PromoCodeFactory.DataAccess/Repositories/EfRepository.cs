using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Database;
using Otus.Teaching.PromoCodeFactory.DataAccess.Exceptions;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        private readonly PromoCodesDbContext _dbContext;

        public EfRepository(PromoCodesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<IEnumerable<T>> GetAllAsync(params string[] includedPropertyNames)
        {
            return await IncludeNavigationPropertiesToQuery(_dbContext, GetDbSet(_dbContext), includedPropertyNames)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, params string[] includedPropertyNames)
        {
            return (await GetAsync(e => e.Id == id, includedPropertyNames))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] includedPropertyNames)
        {
            return await IncludeNavigationPropertiesToQuery(_dbContext, GetDbSet(_dbContext), includedPropertyNames)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<T> CreateAsync(T record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            
            _dbContext.Add(record);
            await SaveEntityAndHandleException(_dbContext, record, EntityState.Added);

            return record;
        }

        public async Task<T> UpdateAsync(T record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            
            _dbContext.Update(record);
            return await SaveEntityAndHandleException(_dbContext, record, EntityState.Modified);
        }

        public async Task<T> DeleteByIdAsync(Guid id)
        {
            var record = await GetByIdAsync(id);
            if (record == null)
                return null;

            _dbContext.Remove(record);
            return await SaveEntityAndHandleException(_dbContext, record, EntityState.Deleted);
        }

        private async Task<T> SaveEntityAndHandleException(DbContext dbContext, T record, EntityState changeType)
        {
            try
            {
                await dbContext.SaveChangesAsync();
                return record;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                if (await GetByIdAsync(record.Id) == null)
                    return null;

                throw new DataInsertionConflictException($"Could not {ConvertChangeTypeToString(changeType)} record, see internal exception for details: {dbx.Message}",
                    dbx, record.Id.ToString());
            }
        }

        private static string ConvertChangeTypeToString(EntityState changeType)
        {
            switch (changeType)
            {
                case EntityState.Deleted:
                    return "delete";
                case EntityState.Modified:
                    return "update";
                case EntityState.Added:
                    return "add";
                default:
                    return "[unsupported modification type]";
            }
        }

        private static DbSet<T> GetDbSet(DbContext dbContext)
        {
            return dbContext.Set<T>();
        }
        
        private static IQueryable<T> IncludeNavigationPropertiesToQuery(
            DbContext dbContext, 
            IQueryable<T> query,
            params string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length < 1)
                return query;
            
            var type = dbContext.Model.GetEntityTypes().FirstOrDefault(e => e.ClrType == typeof(T));
            if (type == null)
                return query;
            
            foreach (var name in propertyNames) 
                query = query.Include(name);
                
            return query;
        }
    }
}