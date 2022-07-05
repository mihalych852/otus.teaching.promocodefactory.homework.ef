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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodeRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PromocodesController(
            IRepository<PromoCode> promocodeRepository,
            IUnitOfWork unitOfWork,
            IRepository<Preference> preferenceRepository, 
            IRepository<Customer> customerRepository, 
            IRepository<Employee> employeeRepository
        )
        {
            _promocodeRepository = promocodeRepository ?? throw new ArgumentNullException(nameof(promocodeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        [ProducesResponseType(typeof(IList<PromoCodeShortResponse>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var entities = await _promocodeRepository.GetAllAsync();

            var result = entities
                .Select(item => new PromoCodeShortResponse
                {
                    Id = item.Id,
                    Code = item.Code,
                    PartnerName = item.PartnerName,
                    ServiceInfo = item.ServiceInfo,
                    BeginDate = item.BeginDate.ToString("s"),
                    EndDate = item.EndDate.ToString("s")
                })
                .ToList();

            return Ok(result);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync([FromBody] GivePromoCodeRequest request)
        {
            var preferenceQuery = await _preferenceRepository.GetAllAsync();
            var preferenceEntity = preferenceQuery.FirstOrDefault(item => item.Name == request.Preference);

            if (preferenceEntity == null)
            {
                return NotFound();
            }

            var customerQuery = await _customerRepository.GetAllAsync();
            var customerEntity = customerQuery
                .FirstOrDefault(item => item
                    .CustomerPreferences
                    .Any(pr => pr.PreferenceId == preferenceEntity.Id)
                );

            if (customerEntity == null)
            {
                return NotFound();
            }

            var employeeQuery = await _employeeRepository.GetAllAsync();
            var employeeEntity = employeeQuery.FirstOrDefault(item => item.Role.Name == "PartnerManager");

            if (employeeEntity == null)
            {
                return NotFound();
            }

            var promocodeEntity = new PromoCode
            {
                Id = Guid.NewGuid(),
                Code = request.PromoCode,
                PartnerName = request.PartnerName,
                ServiceInfo = request.ServiceInfo,
                PreferenceId = preferenceEntity.Id,
                CustomerId = customerEntity.Id,
                PartnerManagerId = employeeEntity.Id
            };

            _promocodeRepository.Add(promocodeEntity);
            await _unitOfWork.SaveAsync();
            
            return NoContent();
        }
    }
}