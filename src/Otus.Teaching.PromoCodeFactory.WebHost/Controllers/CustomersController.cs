using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {

        private readonly IRepository<Customer> _customersRepository;


        public CustomersController(IRepository<Customer> customersRepository)
        {
            _customersRepository = customersRepository;
        }
        
        /// <summary>
        /// Список всех клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetAllCustomersAsync()
        {
            var customers = await _customersRepository.GetAllAsync();
            var response = customers.Select(item => 
                new CustomerShortResponse()
                {
                    Id = item.Id,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName
                });
            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            throw new NotImplementedException();
        }
        
        [HttpPut("{id}")]
        public Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            throw new NotImplementedException();
        }
        
        [HttpDelete]
        public Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            throw new NotImplementedException();
        }
    }
}