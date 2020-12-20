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
        private readonly ICustomerMapper _customerMapper;
        private readonly IPreferenceMapper _preferenceMapper;
        private readonly IPromoCodeMapper _promoCodeMapper;

        public CustomersController(
            IRepository<Customer> customersRepository,
            IRepository<Preference> preferenceRepository,
            ICustomerMapper customerMapper,
            IPreferenceMapper preferenceMapper,
            IPromoCodeMapper promoCodeMapper)
        {
            _customersRepository = customersRepository;
            _preferenceRepository = preferenceRepository;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customersRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            //Получаем список id предпочтений пользователя
            var customerPrefIds = customer.CustomerPreferences
                .Where(cp => cp.CustomerId == customer.Id).Select(cp => cp.PreferenceId);
            
            //Получаем список всех предпочтений
            var preferences = await _preferenceRepository.GetAllAsync();

            var responce = _customerMapper.ToResponse(
                customer,
                preferences
                    .Where(p => customerPrefIds.Contains(p.Id))
                    .Select(_preferenceMapper.ToShortResponse),
                customer.PromoCodes.Select(_promoCodeMapper.ToShortResponse));
            
            return Ok(responce);
        }
        
        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _preferenceRepository.GetAllAsync();
            
            var customer = _customerMapper.FromRequestModel(
                request, preferences.Where(p => request.PreferenceIds.Contains(p.Id)));
            
            await _customersRepository.AddAsync(customer);
            
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new {Id = customer.Id}, customer.Id);
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