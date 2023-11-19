using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

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

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository, 
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodesRepository.GetAllAsync();

            var response = promoCodes.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo,
                CustomerId = x.CustomerId
            }).ToList();

            return Ok(response);

        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<object>> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preference = await _preferenceRepository
                .GetByIdAsync(request.PreferenceId);

            var customers = await _customerRepository.GetAllAsync();

            var promoCode = new PromoCode()
            {
                ServiceInfo = request.ServiceInfo,
                PartnerName = request.PartnerName,
                Code = request.PromoCode,
                Preference = preference
            };
            promoCode.CustomerId = customers.Select(c => c.Preferences.Where(p => p.PreferenceId == request.PreferenceId).First()).First().CustomerId;

            await _promoCodesRepository.AddAsync(promoCode);

            return Ok( new { id = promoCode.Id });
        }
    }
}