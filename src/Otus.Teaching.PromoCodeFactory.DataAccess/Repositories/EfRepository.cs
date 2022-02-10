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
        IRepository<T> 
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
        public async Task<T> CreateAsync(T item)
        {
            item.Id = Guid.NewGuid();
            ((IList<T>)_dataContext).Add(item);
            _dataContext.SaveChanges();
            return await Task.FromResult(item);
        }

        /// <summary>
        /// Изменение данных по ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns><T></returns>
        public async Task<T> UpdateAsync(T item)
        {
            //определяем индекс изменяемой записи
            var index = ((IList<T>)_dataContext).IndexOf(_dataContext.Set<T>().Where(x => x.Id == item.Id).First());
            ((IList<T>)_dataContext)[index] = item;
            _dataContext.SaveChanges();
            return await Task.FromResult(item);

        }

        /// <summary>
        /// Удаление записи по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            ((List<T>)_dataContext).RemoveAll(i => i.Id == id);
            _dataContext.SaveChanges();
            return await Task.CompletedTask;
        }

    }
}
