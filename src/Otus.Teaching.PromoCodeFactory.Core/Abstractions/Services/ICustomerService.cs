using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync(); 
        Task<Customer> GetAsync(Guid id);
        Task<Guid> CreateAsync(CreateOrEditCustomerDto customer);
        Task<bool> UpdateAsync(Guid id, CreateOrEditCustomerDto customer);
        Task<bool> DeleteAsync(Guid id);
    }
}
