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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Customer> _customerRepository;
 
        public PromocodesController(
            IRepository<PromoCode> promoCodeRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<Customer> customerRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodeRepository.GetAllAsync();

            var promoCodesList = promoCodes.Select(pc =>
                new PromoCodeShortResponse
                {
                    Id = pc.Id,
                    Code = pc.Code,
                    ServiceInfo = pc.ServiceInfo,
                    BeginDate = pc.BeginDate.ToShortDateString(),
                    EndDate = pc.EndDate.ToShortDateString(),
                    PartnerName = pc.PartnerName
                }).ToList();

            return Ok(promoCodesList);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            if (request.Preference is null || request.PromoCode is null || request.PartnerName is null)
            {
                return BadRequest();
            }

            var preferenceId = Guid.Parse(request.Preference);
            var preference = await _preferenceRepository.GetByIdAsync(preferenceId);

            if (preference is null)
            {
                return NotFound("Preference not found");
            }

            var customers = await _customerRepository.GetAllAsync();

            if (!preference.CustomerPreferences.Any())
            {
                return NotFound("No customers for preference");
            }

            var promoCodes = preference.CustomerPreferences
                .Select(c => new PromoCode
                {
                    Id = Guid.NewGuid(),
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    Code = request.PromoCode,
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    PreferenceId = preferenceId,
                    CustomerId = c.CustomerId
                });

            await _promoCodeRepository.AddRangeAsync(promoCodes);

            return Ok();
        }

    }
}