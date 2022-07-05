using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
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
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IRepository<Customer> customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Выгрузить информацию по всем клиентам
        /// </summary>
        [ProducesResponseType(typeof(IList<CustomerShortResponse>),StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IList<CustomerShortResponse>>> GetCustomersAsync()
        {
            var entities = await _customerRepository.GetAllAsync();

            var result = entities
                .Select(item => new CustomerShortResponse
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email
                })
                .ToList();

            return Ok(result);
        }
        
        /// <summary>
        /// Получить информацию о клиенте по его идентификатору
        /// </summary>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var entity = await _customerRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var result = new CustomerResponse
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PromoCodes = entity
                    .Promocodes
                    .Select(item => new PromoCodeShortResponse
                    {
                        BeginDate = item.BeginDate.ToString(),
                        Code = item.Code,
                        EndDate = item.EndDate.ToString(),
                        Id = item.Id,
                        PartnerName = item.PartnerName,
                        ServiceInfo = item.ServiceInfo
                    })
                    .ToList(),
                Preferences = entity
                    .CustomerPreferences
                    .Select(item => new PreferenceResponse
                    {
                        Id = item.Preference.Id,
                        Name = item.Preference.Name
                    })
                    .ToList()
            };

            return Ok(result);
        }
        
        /// <summary>
        /// Добавить нового клиента
        /// </summary>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateOrEditCustomerRequest request)
        {
            var customerId = Guid.NewGuid();
            
            var entity = new Customer
            {
                Id = customerId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
            
            _customerRepository.Add(entity);
            await _unitOfWork.SaveAsync();

            entity.CustomerPreferences = request
                    .PreferenceIds
                    .Select(preferenceId => new CustomerPreference
                    {
                        CustomerId = customerId,
                        PreferenceId = preferenceId
                    })
                    .ToList();
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Обновить информацию о клиенте
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="request">Данные, которые надо обновить</param>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, [FromBody] CreateOrEditCustomerRequest request)
        {
            var entity = await _customerRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Email = request.Email;
            entity.CustomerPreferences.Clear();

            _customerRepository.Update(entity);
            await _unitOfWork.SaveAsync();

            entity.CustomerPreferences = request
                .PreferenceIds
                .Select(preferenceId => new CustomerPreference
                {
                    CustomerId = id,
                    PreferenceId = preferenceId
                })
                .ToList();
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить клиента по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор клиента</param>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var entityToDelete = await _customerRepository.GetByIdAsync(id);

            if (entityToDelete == null)
            {
                return NotFound();
            }
            
            _customerRepository.Remove(entityToDelete);
            await _unitOfWork.SaveAsync();
            
            return NoContent();
        }
    }
}