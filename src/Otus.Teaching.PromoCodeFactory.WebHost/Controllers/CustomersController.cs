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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promoRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promoRepository = promoRepository;
        }

        /// <summary>
        /// Получить данные всех клиентов
        /// </summary>
        /// <returns>Список, содержащий наборы данных клиентов</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var customersModelList = customers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                }).ToList();

            return Ok(customersModelList);
        }

        /// <summary>
        /// Получить данные клиента по id
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Данные клиента</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            var customerModel = new CustomerResponse(customer);

            return Ok(customerModel);
        }

        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        /// <param name="request">Данные нового клиента</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            var preferences = await _preferenceRepository.GetListByIdsAsync(request.PreferenceIds);
            customer.Preferences = preferences.ToList();

            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, null);
        }

        /// <summary>
        /// Редактирование клиента по id
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            var preferences = _preferenceRepository.GetListByIdsAsync(request.PreferenceIds);
            customer.Preferences = preferences.Result.ToList();

            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        /// <summary>
        /// Удаление клиента по id
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            //if (customer.Preferences != null &&  customer.Preferences.Count > 0)
            //{
            //    await _preferenceRepository.DeleteRangeAsync(customer.Preferences);
            //}

            if(customer.Promocodes != null &&  customer.Promocodes.Count > 0)
            {
                await _promoRepository.DeleteRangeAsync(customer.Promocodes);
            }

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}