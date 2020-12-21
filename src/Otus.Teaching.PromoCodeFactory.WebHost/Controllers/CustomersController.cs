using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PreferenceMapper;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PromoCodeMapper;
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
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly ICustomerMapper _customerMapper;
        private readonly IPreferenceMapper _preferenceMapper;
        private readonly IPromoCodeMapper _promoCodeMapper;

        public CustomersController(
            IRepository<Customer> customersRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<CustomerPreference> customerPreferenceRepository,
            ICustomerMapper customerMapper,
            IPreferenceMapper preferenceMapper,
            IPromoCodeMapper promoCodeMapper)
        {
            _customersRepository = customersRepository;
            _preferenceRepository = preferenceRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _customerMapper = customerMapper;
            _preferenceMapper = preferenceMapper;
            _promoCodeMapper = promoCodeMapper;
        }
        
        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetAllCustomersAsync()
        {
            var customers = await _customersRepository.GetAllAsync();
            var response = customers
                .Select(item => _customerMapper.ToShortResponse(item));
            return Ok(response);
        }
        
        /// <summary>
        /// Получить клиента по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customersRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            //Получаем список предпочтений клиента
            var preferencesShortResponses = customer.CustomerPreferences
                .Select(cp => _preferenceMapper.ToShortResponse(cp.Preference));

            var responce = _customerMapper.ToResponse(
                customer,
                preferencesShortResponses,
                customer.PromoCodes.Select(_promoCodeMapper.ToShortResponse));
            
            return Ok(responce);
        }
        
        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _preferenceRepository.GetAllAsync();
            
            var customer = _customerMapper.FromRequestModel(
                request, preferences.Where(p => request.PreferenceIds.Contains(p.Id)));
            
            await _customersRepository.AddAsync(customer);
            
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new {Id = customer.Id}, customer.Id);
        }
        /// <summary>
        /// Редактировать клиента по Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomerByIdAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customersRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            var preferences = await _preferenceRepository.GetAllAsync();
            
            //удаляем старые предпочтения пользователя
            var customerPreference = await _customerPreferenceRepository.GetAllAsync();
            await _customerPreferenceRepository.DeleteRangeAsync(
                customerPreference.Where(cp => cp.CustomerId == customer.Id));
            
            _customerMapper.FromRequestModel(
                request,
                preferences.Where(p => request.PreferenceIds.Contains(p.Id)),
                customer);

            await _customersRepository.UpdateAsync(customer);

            return Ok();
        }
        
        /// <summary>
        /// Удаление клиента по id. Также удаляются все выданные клиенту промокоды.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomerByIdAsync(Guid id)
        {
            var customer = await _customersRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customersRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}