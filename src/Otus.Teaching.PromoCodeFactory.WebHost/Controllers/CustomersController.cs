using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;

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
        private readonly IEfRepository<Customer> _customerRepository;
        private readonly IEfRepository<PromoCode> _promoCodeRepository;
        private readonly IEfRepository<Preference> _preferenceRepository;

        public CustomersController(IEfRepository<Customer> customerRepository, IEfRepository<PromoCode> promoCodeRepository, IEfRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
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
            return customersModelList;
        }
        
        /// <summary>
        /// Получение информации о клиенте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            var customerModel = new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PromoCodes = customer.PromoCodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    BeginDate = x.BeginDate.ToShortDateString(),
                    EndDate = x.EndDate.ToShortDateString(),
                    PartnerName = x.PartnerName,
                    ServiceInfo = x.ServiceInfo
                }).ToList(),
            };

            Console.WriteLine(customerModel);
            return customerModel;
        }
        
        /// <summary>
        /// Создание нового клиента вместе с его препочтениями
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferenceModel = await _preferenceRepository.GetAllAsync();
            var preferences = new List<Preference>();
            var promoCodes = new List<PromoCode>();
            foreach (var id in request.PreferenceIds )
            {
                preferences.Add(preferenceModel.FirstOrDefault(x => x.Id == id));
            }

            var customerModel = new Customer()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Preferences = preferences.ToArray(),
                PromoCodes = promoCodes
            };
            await _customerRepository.CreateAsync(customerModel);
            return Ok();
        }

        /// <summary>
        /// Обновить данные клиента вместе с его предпочтениями
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customerModel = await _customerRepository.GetByIdAsync(id);
            var preferenceModel = await _preferenceRepository.GetAllAsync();
            var preferences = request.PreferenceIds.ToArray();

            customerModel(x =>
            {
                x.Email = request.Email;
                x.FirstName = request.FirstName;
                x.LastName = request.LastName;
                x.Preferences = preferences.ToArray();
            });
            await _customerRepository.UpdateAsync(customerModel);
            return Ok();
        }

        /// <summary>
        /// Удаление клиента вместе с выданными ему промокодами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Customer>> DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
            await _promoCodeRepository.DeleteAsync(x => x.owner.id = id);
            return Ok();
        }
    }
}