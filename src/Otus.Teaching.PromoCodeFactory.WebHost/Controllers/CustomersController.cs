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
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var customersModelList = customers.Select(c =>
                new CustomerShortResponse
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    IsVerified = c.IsVerified
                }).ToList();

            return customersModelList;
        }

        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerModel = new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                IsVerified = customer.IsVerified,
                PromoCodes = customer.PromoCodes?
                    .Select(pc => new PromoCodeShortResponse
                    {
                        Id = pc.Id,
                        Code = pc.Code,
                        ServiceInfo = pc.ServiceInfo,
                        BeginDate = pc.BeginDate.ToShortDateString(),
                        EndDate = pc.EndDate.ToShortDateString(),
                        PartnerName = pc.PartnerName
                    }).ToList()
            };

            if (customer.CustomerPreferences?.Any() == true)
            {
                customerModel.Preferences = new List<PreferenceResponse>();
                foreach (var customerPreference in customer.CustomerPreferences)
                {
                    var preference = await _preferenceRepository.GetByIdAsync(customerPreference.PreferenceId);
                    if (preference != null)
                    {
                        customerModel.Preferences.Add(new PreferenceResponse()
                        {
                            Id = preference.Id,
                            Name = preference.Name
                        });
                    }
                }
            }

            return customerModel;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            if (request.Email is null || request.FirstName is null)
            {
                return BadRequest();
            }
            
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CustomerPreferences = request.PreferenceIds?
                    .Select(preferenceId => new CustomerPreference
                    {
                        CustomerId = customerId,
                        PreferenceId = preferenceId
                    }).ToList()
            };

            await _customerRepository.AddAsync(customer);

            return Ok(new {customer.Id});
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            if (request.Email is null || request.FirstName is null)
            {
                return BadRequest();
            }
            
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.CustomerPreferences = request.PreferenceIds?
                .Select(preferenceId => new CustomerPreference
                {
                    CustomerId = customer.Id,
                    PreferenceId = preferenceId
                }).ToList();

            await _customerRepository.UpdateAsync(customer);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerRepository.DeleteAsync(customer.Id);
            return Ok();
        }
    }
}