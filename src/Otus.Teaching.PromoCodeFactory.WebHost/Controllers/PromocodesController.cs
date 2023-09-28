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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository, IRepository<CustomerPreference> customerPreferenceRepository
            , IRepository<Preference> preferenceRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodeRepository.GetAllAsync();
            return new ActionResult<IEnumerable<PromoCodeShortResponse>>(
                promoCodes.Select(x => new PromoCodeShortResponse
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.StartDate.ToLongTimeString(),
                    EndDate = x.EndDate.ToLongTimeString(),
                    PartnerName = x.PartnerName
                }));
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preference = (await _preferenceRepository.GetAllAsync()).FirstOrDefault(x=>string.Equals(x.Name, request.Preference));
            if (preference == null) return new BadRequestResult();
            var usersId = (await _customerPreferenceRepository.GetAllAsync()).Where(x => x.PreferenceId == preference.Id);
            await _promoCodeRepository.AddRange(usersId.Select(x => new PromoCode
            {
                Id = Guid.NewGuid(),
                Code = Guid.NewGuid().ToString("d"),
                ServiceInfo = request.ServiceInfo,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(2),
                PartnerName = request.PartnerName,
                PreferenceId = preference.Id
            }));
            return new OkResult();
        }
    }
}