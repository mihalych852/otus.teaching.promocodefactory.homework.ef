using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Exceptions;
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
        private readonly IMapper _mapper;

        public CustomersController(
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Возвращает список всех клиентов в короткой форме
        /// </summary>
        /// <returns>Перечень коротких форм клиентов</returns>
        [HttpGet]
        public async Task<IEnumerable<CustomerShortResponse>> GetCustomersAsync()
        {
            return (await _customerRepository.GetAllAsync())
                .Select(c => _mapper.Map<CustomerShortResponse>(c));
        }
        
        /// <summary>
        /// Возвращает клиента по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Клиент с предпочтениями</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync([Required] Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id, nameof(Customer.Preferences), nameof(Customer.PromoCodes));
            if (customer == null)
                return NotFound();

            return _mapper.Map<CustomerResponse>(customer);
        }
        
        /// <summary>
        /// Создает нового клиента
        /// </summary>
        /// <param name="request">Запрос на создание</param>
        /// <returns>Созданный клиент</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync([Required][FromBody] CreateOrEditCustomerRequest request)
        {
            try
            {
                var newCustomer = _mapper.Map<Customer>(request);

                if (request.PreferenceIds != null && request.PreferenceIds.Count > 0)
                    newCustomer.Preferences = (await _preferenceRepository.GetAsync(e => request.PreferenceIds.Contains(e.Id)))
                        .ToList();

                return _mapper.Map<CustomerResponse>(await _customerRepository.CreateAsync(newCustomer));
            }
            catch (DataInsertionConflictException conflictException)
            {
                return Conflict("Conflicting identifiers: " + string.Join(", ", conflictException.ConflictingIdentifiers));
            }
        }
        
        /// <summary>
        /// Обновляет выбранного клиента
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns>Обновленный клиент</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> EditCustomersAsync([Required] Guid id, [Required][FromBody] CreateOrEditCustomerRequest request)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id, nameof(Customer.Preferences));
                if (existingCustomer == null)
                    return NotFound();
                
                var customer = _mapper.Map<Customer>(request);
                customer.Id = id;

                await UpdateCustomerDataAsync(existingCustomer, request);
                
                customer = await _customerRepository.UpdateAsync(existingCustomer);
                if (customer == null)
                    return NotFound();
                
                return _mapper.Map<CustomerResponse>(customer);
            }
            catch (DataInsertionConflictException conflictException)
            {
                return BadRequest(conflictException.Message);
            }
        }
        
        /// <summary>
        /// Удаляет клиента по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <returns>Удаленный клиент</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> DeleteCustomer(Guid id)
        {
            try
            {
                var deletedCustomer = await _customerRepository.DeleteByIdAsync(id);
                if (deletedCustomer == null)
                    return NotFound();

                return _mapper.Map<CustomerResponse>(deletedCustomer);
            }
            catch (DataInsertionConflictException conflictException)
            {
                return BadRequest(conflictException.Message);
            }
        }

        private async Task UpdatePreferencesDataAsync(Customer customer, CreateOrEditCustomerRequest request)
        {
            if (request.PreferenceIds != null && request.PreferenceIds.Count > 0)
            {
                var requestPreferences = await _preferenceRepository.GetAsync(e => request.PreferenceIds.Contains(e.Id));
                customer.Preferences ??= new List<Preference>();
                
                customer.Preferences.Where(p => !requestPreferences.Any(rp => rp.Id == p.Id))
                    .ToList().ForEach(p => customer.Preferences.Remove(p));
                
                requestPreferences.Where(rp => !customer.Preferences.Any(p => p.Id == rp.Id))
                    .ToList().ForEach(rp => customer.Preferences.Add(rp));
            }
            else
                customer.Preferences?.Clear();
        }

        private async Task UpdateCustomerDataAsync(Customer oldCustomerData, CreateOrEditCustomerRequest newCustomerData)
        {
            await UpdatePreferencesDataAsync(oldCustomerData, newCustomerData);
            oldCustomerData.Email = newCustomerData.Email;
            oldCustomerData.FirstName = newCustomerData.FirstName;
            oldCustomerData.LastName = newCustomerData.LastName;
        }
    }
}