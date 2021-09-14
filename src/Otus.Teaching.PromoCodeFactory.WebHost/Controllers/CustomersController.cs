using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IMapper _mapper;

        public CustomersController(IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<PromoCode> promoCodeRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promoCodeRepository = promoCodeRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<CustomerShortResponse>> GetCustomersAsync()
        {
            var models = await _customerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerShortResponse>>(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerResponse>(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = _mapper.Map<Customer>(request);
            foreach (var preferenceId in request.PreferenceIds)
            {
                var preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                customer.Preferences.Add(preference);
            }

            if (!await _customerRepository.AddAsync(customer))
                return BadRequest();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var dbCustomer = await _customerRepository.GetByIdAsync(id);
            dbCustomer.Email = request.Email;
            dbCustomer.FirstName = request.FirstName;
            dbCustomer.LastName = request.LastName;
            dbCustomer.Preferences ??= new List<Preference>();
            dbCustomer.Preferences = dbCustomer.Preferences
                .Where(p => request.PreferenceIds.Contains(p.Id))
                .ToList();

            var preferencesToGet = request.PreferenceIds
                .Except(dbCustomer.Preferences.Select(p => p.Id));

            foreach (var preferenceId in preferencesToGet)
            {
                var preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                dbCustomer.Preferences.Add(preference);
            }

            if (!await _customerRepository.UpdateAsync(dbCustomer))
                return BadRequest();

            return Ok();

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            foreach (var promoCodeId in customer.PromoCodes.Select(p => p.Id).ToList())
            {
                await _promoCodeRepository.DeleteAsync(promoCodeId);
            }

            if (await _customerRepository.DeleteAsync(customer.Id))
                return Ok();

            return BadRequest();
        }
    }
}