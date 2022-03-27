using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;



namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> :
        IEfRepository<T>
        where T:BaseEntity
    {
        private readonly DataContext _dataContext;

        public EfRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var qitems = _dataContext.Set<T>();
            return await Task.FromResult(qitems.ToList());
        }

        /// <summary>
        /// Поиск по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(Guid id)
        {
            var qitem = _dataContext.Set<T>().FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(qitem);
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="data"></param>
        /// <returns><T></returns>
        public Task CreateAsync(T item)
        {
            item.Id = Guid.NewGuid();
            _dataContext.AddAsync(item);
            return _dataContext.SaveChangesAsync();
            
        }

        /// <summary>
        /// Изменение данных по ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns><T></returns>
        public Task UpdateAsync(T item)
        {
            _dataContext.Update(item);
            return _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление записи по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteAsync(Guid id)
        {
            _dataContext.Remove(_dataContext.Set<T>().Find(id));
            return _dataContext.SaveChangesAsync();
        }

    }
}
