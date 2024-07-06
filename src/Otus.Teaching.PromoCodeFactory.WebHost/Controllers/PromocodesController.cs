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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoRepository;

        public PromocodesController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promoRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promoRepository = promoRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoRepository.GetAllAsync();

            var promoCodesModelList = promoCodes.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName,

                }).ToList();

            return Ok(promoCodesModelList);
        }

        /// <summary>
        /// Получить промокод по id
        /// </summary>
        /// <param name="id">Идентификатор промокода</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PromoCodeShortResponse>> GetPromoCodeAsync(Guid id)
        {
            var promoCode = await _promoRepository.GetByIdAsync(id);

            if (promoCode == null)
                return NotFound();

            var promoCodeModel = new PromoCodeShortResponse(promoCode);

            return Ok(promoCodeModel);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var promoCode = new PromoCode()
            {
                Id = Guid.NewGuid(),
                ServiceInfo = request.ServiceInfo,
                PartnerName = request.PartnerName,
                Code = request.PromoCode,
            };

            Guid preferenceId;
            if (Guid.TryParse(request.Preference, out preferenceId))
            {
                var preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                if (preference != null)
                {
                    promoCode.Preference = preference;
                    var customers = await _customerRepository.GetAllAsync();
                    foreach(var customer in customers.Where(x => x.Preferences.Contains(preference)))
                    {
                        if (customer.Promocodes != null)
                        {
                            customer.Promocodes.Add(promoCode);
                        }
                        else
                        {
                            customer.Promocodes = new List<PromoCode> { promoCode };
                        }
                        await _customerRepository.UpdateAsync(customer);
                    }
                }
            }

            await _promoRepository.AddAsync(promoCode);

            return CreatedAtAction(nameof(GetPromoCodeAsync), new { id = promoCode.Id }, null);
        }
    }
}