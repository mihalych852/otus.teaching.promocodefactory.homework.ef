using System;
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
        private readonly IRepository<Customer> _custRepository;
        private readonly IRepository<Preference> _prefRepository;

        public CustomersController(IRepository<Customer> custRepository, IRepository<Preference> prefRepository)
        {
            _custRepository = custRepository;
            _prefRepository = prefRepository;
        }

        /// <summary>
        /// Получить всех покупателей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortDTO>> GetCustomersAsync()
        {
            var customers =  await _custRepository.GetAllAsync();
            var response = customers
                .Select(x => new CustomerShortDTO(x)).ToList();

            return Ok(response);
        }


        /// <summary>
        /// Получить покупателей по ИД
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerAsync(Guid id)
        {
            var customer =  await _custRepository.GetByIdAsync(id);

            if (customer == null)
                NotFound();

            var response = new CustomerDTO()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,

                Preferences = customer.Preferences?.Select(x => new PreferenceDTO()
                {
                    Id = x.PreferenceId,
                    Name = x.Preference.Name
                }).ToList()
            };

            return Ok(response);
        }


        /// <summary>
        /// Создать покупателя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _prefRepository.FilterByIdsAsync(request.PreferenceIds);

            var customer = new Customer()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            customer.Preferences = preferences?.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            }).ToList();

            await _custRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetCustomerAsync), new {id = customer.Id}, null);
        }


        /// <summary>
        /// Отредактировать покупателя
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _custRepository.GetByIdAsync(id);
            
            if (customer == null)
                return NotFound();

            var preferences = await _prefRepository.FilterByIdsAsync(request.PreferenceIds);

            customer.Email = request.Email;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Preferences?.Clear();

            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                CustomerId = customer.Id,
                Preference = x,
                PreferenceId = x.Id
            }).ToList();

            await _custRepository.UpdateAsync(customer);

            return NoContent();
        }


        /// <summary>
        /// Удалить покупателя
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomerAsync(Guid id)
        {
            var customer = await _custRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            customer.Preferences?.Clear();
            customer.PromoCodes?.Clear();

            await _custRepository.DeleteAsync(customer);

            return NoContent();
        }
    }
}