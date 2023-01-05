using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly PromocodeFactoryDb _db;

        public CustomerRepository(PromocodeFactoryDb db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            IEnumerable<Customer> result = _db.Customers;
            return await Task.FromResult(result);
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _db.Customers
                .Include(c => c.Preferences)
                .Include(c => c.PromoCodes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> CreateAsync(Customer entity)
        {
            _db.Customers.Add(entity);
            await _db.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task UpdateAsync(Customer entity)
        {
            var updateCustomer = _db.Customers.FirstOrDefault(e => e.Id == entity.Id);
            if (updateCustomer != null)
            {
                updateCustomer.Email = entity.Email;
                updateCustomer.FirstName = entity.FirstName;
                updateCustomer.LastName = entity.LastName;
                updateCustomer.Preferences = entity.Preferences;
                updateCustomer.PromoCodes = entity.PromoCodes;
            }
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Employee emp = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
            _db.Employees.Remove(emp);
            return true;
        }
    }
}
