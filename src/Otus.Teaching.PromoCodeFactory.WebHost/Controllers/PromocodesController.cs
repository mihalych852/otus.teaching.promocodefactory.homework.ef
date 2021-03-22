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
    public class PromocodesController : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoRepository;
        private readonly IRepository<Preference> _prefRepository;
        private readonly IRepository<Customer> _custRepository;

        public PromocodesController(IRepository<PromoCode> promoRepository, IRepository<Preference> prefRepository, IRepository<Customer> custRepository)
        {
            _promoRepository = promoRepository;
            _prefRepository = prefRepository;
            _custRepository = custRepository;
        }
        
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortDTO>>> GetPromocodesAsync()
        {
            var promos = await _promoRepository.GetAllAsync();

            var response = promos.Select(x => new PromoCodeShortDTO()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            if (request == null)
                NotFound();

            var preferences = await _prefRepository.FilterAsync(x => x.Name == request.Preference);

            var preference = preferences.FirstOrDefault();

            if (preference != null)
            {
                var code = new PromoCode()
                {
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    PreferenceId = preference.Id
                };

                await _promoRepository.AddAsync(code);

                var custs = await _custRepository.FilterAsync(x => x.Preferences.Any(y => y.Preference.Id == preference.Id));

                foreach (var cust in custs)
                {
                    cust.PromoCodes.Add(code);
                }
            }
            else {
                NoContent();
            }

            return Ok();
        }
    }
}