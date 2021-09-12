using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CustomersController(IRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список всех клиентов в кратком представлении.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync().ConfigureAwait(false);
            var customersDto = _mapper.Map<IEnumerable<CustomerShortResponse>>(customers);
            return Ok(customersDto);
        }
        
        /// <summary>
        /// Получить полную информацию по клиенту по его id
        /// </summary>
        /// <param name="id">id клиента</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            var customerDto = _mapper.Map<CustomerResponse>(customer);
            return Ok(customerDto);
        }
        
        /// <summary>
        /// Создать нового клиента с предпочтениями.
        /// </summary>
        /// <param name="request">Информация о клиенте.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = _mapper.Map<Customer>(request);
            customer.CustomerPreferences = CreateCustomerPreferences(request.PreferenceIds);
            await _customerRepository.AddAsync(customer).ConfigureAwait(false);
            return Ok();
        }
        
        /// <summary>
        /// Обновить информацию о клиенте и его предпочтениях.
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <param name="request">Информация о клиенте</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (customer == null)
            {
                return NotFound();
            }
            
            customer.CustomerPreferences = CreateCustomerPreferences(request.PreferenceIds);
            customer.Email = request.Email;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;

            await _customerRepository.UpdateAsync(customer).ConfigureAwait(false);
            return Ok();
        }
        
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(customer).ConfigureAwait(false);
            return Ok();
        }

        private static List<CustomerPreference> CreateCustomerPreferences(IEnumerable<Guid> preferencesIds)
        {
            var preferences = new List<CustomerPreference>();
            foreach (var preferenceId in preferencesIds)
            {
                preferences.Add(new CustomerPreference()
                {
                    PreferenceId = preferenceId
                });
            }

            return preferences;
        }
    }
}