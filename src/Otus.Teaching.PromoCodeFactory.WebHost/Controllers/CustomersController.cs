using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers {
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<PromoCode> promoCodeRepository,
            IRepository<CustomerPreference> customerPreferenceRepository,
            IRepository<Preference> preferenceRepository) {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CustomerShortResponse>> GetCustomersAsync() {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(x => new CustomerShortResponse {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            });
        }

        /// <summary>
        /// Получить клиента по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id) {
            var customers = await _customerRepository.GetByIdAsync(id);
            var promoCodes = (await _promoCodeRepository.GetAllAsync()).Where(x => x.CustomerId == id);

            return new CustomerResponse {
                Id = customers.Id,
                FirstName = customers.FirstName,
                LastName = customers.LastName,
                Email = customers.Email,
                PromoCodes = promoCodes.Select(x => new PromoCodeShortResponse {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.StartDate.ToLongDateString(),
                    EndDate = x.EndDate.ToLongDateString(),
                    PartnerName = x.PartnerName
                }).ToList()
            };
        }

        /// <summary>
        /// Cоздать клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request) {
            var preferences = await _preferenceRepository.GetAllAsync();
            if (preferences.Any(x => !request.PreferenceIds.Contains(x.Id)))
                throw new Exception("Add preferences");
            var newCustomer = new Customer {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            await _customerRepository.Add(newCustomer);

            await _customerPreferenceRepository.AddRange(request.PreferenceIds.Select(x => new CustomerPreference {
                Id = Guid.NewGuid(),
                CustomerId = newCustomer.Id,
                PreferenceId = x,
            }));
            return new OkResult();
        }


        /// <summary>
        /// Изменить данные клиента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request) {
            var preferences = await _preferenceRepository.GetAllAsync();
            if (preferences.Any(x => !request.PreferenceIds.Contains(x.Id)))
                return new NotFoundResult();

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return new NotFoundResult();
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            await _customerRepository.Update(customer);

            var currentPreferences = (await _customerPreferenceRepository.GetAllAsync())
                .Where(x => x.CustomerId == customer.Id).Select(x => x.PreferenceId)
                .ToList();
            var removeCollection = currentPreferences.Except(request.PreferenceIds);
            await _customerPreferenceRepository.RemoveRange(removeCollection);

            var addRange = request.PreferenceIds.Except(currentPreferences);
            var customerPreferences = addRange.Select(x => new CustomerPreference {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                PreferenceId = x,
            });

            await _customerPreferenceRepository.AddRange(customerPreferences);
            return new OkResult();
        }

        /// <summary>
        /// Удалить клиента по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id) {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return new NotFoundResult();
            await _customerRepository.DeleteByIdAsync(id);
            var promos = _promoCodeRepository.GetAllAsync().Result.Where(x => x.CustomerId == id);
            await _promoCodeRepository.RemoveRange(promos.Select(x => x.Id));
            return new OkResult();
        }
    }
}