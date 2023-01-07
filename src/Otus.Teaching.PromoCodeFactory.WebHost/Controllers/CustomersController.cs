using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IRepository<Customer> _dbCustomers;
        private readonly IRepository<Preference> _dbPreference;
        private readonly IMapper _mapper;

        /// <summary>
        /// CustomersController Constructor
        /// </summary>
        /// <param name="db"></param>
        public CustomersController(IRepository<Customer> dbCustomers, IRepository<Preference> dbPreference, IMapper mapper)
        {
            _dbCustomers = dbCustomers;
            _dbPreference = dbPreference;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all customers async
        /// </summary>
        /// <returns>IEnumerable of CustomerShortResponse</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _dbCustomers.GetAllAsync();
            var customersDto = _mapper.Map<IEnumerable<CustomerShortResponse>>(customers);
            return Ok(customersDto);
        }

        /// <summary>
        /// Get the Customer by Id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns>customerDto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _dbCustomers.GetByIdAsync(id);
            var customerDto = _mapper.Map<CustomerResponse>(customer);
            return Ok(customerDto);
        }
        
        /// <summary>
        /// Create a Customer async
        /// </summary>
        /// <param name="request"></param>
        /// <returns>status code</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var newCustomer = _mapper.Map<Customer>(request);

            if (newCustomer.Preferences.Count > 1)
            {
                var preferenceIds = newCustomer.Preferences.Select(p => p.Id).ToArray();
                var preferences = await _dbPreference.GetByIdAsync(preferenceIds);
                newCustomer.Preferences = preferences.ToList();
            }

            await _dbCustomers.CreateAsync(newCustomer);
            return Ok();
        }
        
        /// <summary>
        /// Edit the Customer async
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var editedCustomer = _mapper.Map<Customer>(request);
            var customer = await _dbCustomers.GetByIdAsync(id);
            // CreateOrEditCustomerRequest don't have Promocodes property.
            editedCustomer.PromoCodes = customer.PromoCodes;
            await _dbCustomers.UpdateAsync(editedCustomer);
            return Ok();
        }
        
        /// <summary>
        /// Delete the Customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _dbCustomers.DeleteAsync(id);
            return result ? Ok() : BadRequest();
        }
    }
}