using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {

        /// <summary>
        /// получить все строки из таблицы
        /// </summary>
        /// <returns>список записей сущности</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// получитб запись по ИД
        /// </summary>
        /// <param name="id">ИД GUID</param>
        /// <returns>строка из сущности</returns>
        Task<T> GetByIdAsync(Guid id);
        /// <summary>
        /// создание записи в сущности
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>возврат созданой записи</returns>
        Task<T> CreateAsync(T entity);
        /// <summary>
        /// изменение существующей записи
        /// </summary>
        /// <param name="entity">в качестве параметра запись в сущности</param>
        /// <returns></returns>
        Task UpdateAsync(T entity);
        /// <summary>
        /// удаление записи 
        /// </summary>
        /// <param name="id">ИД записи</param>
        /// <returns>возврат признака удаления записи</returns>
        Task<bool> DeleteAsync(Guid id);
        /// <summary>
        /// навигация по специфическому свойству
        /// </summary>
        /// <param name="propertyName">наименование свойства</param>
        /// <param name="id">ИД записи</param>
        /// <returns></returns>
        Task<T> GetEntityWithLoadedSpecificNavigationProperty(string propertyName, Guid id);
    }
}