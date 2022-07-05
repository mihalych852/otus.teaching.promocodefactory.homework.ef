using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.DbContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        private readonly SqlLiteDbContext _sqlLiteDbContext;
        private readonly DbSet<T> _database;

        public EfRepository(SqlLiteDbContext sqlLiteDbContext)
        {
            _sqlLiteDbContext = sqlLiteDbContext ?? throw new ArgumentNullException(nameof(sqlLiteDbContext));

            _database = _sqlLiteDbContext.Set<T>();
        }

        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.FromResult(_database.AsQueryable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_database.Find(id));
        }
        
        public void Update(T entity)
        {
            if (_sqlLiteDbContext.Entry(entity).State == EntityState.Detached)
            {
                _database.Attach(entity);
            }

            _database.Update(entity);
        }
        
        public void Add(T entity)
        {
            if (_sqlLiteDbContext.Entry(entity).State == EntityState.Detached)
            {
                _database.Attach(entity);
            }

            _database.Add(entity);
        }
        
        public void Remove(T entity)
        {
            if (_sqlLiteDbContext.Entry(entity).State == EntityState.Detached)
            {
                _database.Attach(entity);
            }

            _database.Remove(entity);
        }
    }
}