using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IPreferencesRepository _preferencesRepository;
        private readonly IMapper _mapper;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository,
            ICustomersRepository customersRepository,
            IPreferencesRepository preferencesRepository,
            IMapper mapper)
        {
            _promoCodesRepository = promoCodesRepository;
            _customersRepository = customersRepository;
            _preferencesRepository = preferencesRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCodeShortResponse>>> GetPromoCodesAsync()
        {
            var promoCodes = await _promoCodesRepository.GetAllAsync();
            var promoCodesDto = _mapper.Map<IEnumerable<PromoCodeShortResponse>>(promoCodes);
            return Ok(promoCodesDto);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            var promoCode = _mapper.Map<PromoCode>(request);

            var preference = await _preferencesRepository.GetPreferenceByName(request.Preference).ConfigureAwait(false);

            if (preference == null)
            {
                return NotFound("Preference not found");
            }
            
            promoCode.PreferenceId = preference.Id;

            var customersIds = (await _customersRepository.GetCustomersIdsByPreferenceId(preference.Id).ConfigureAwait(false)).ToList();

            if (!customersIds.Any())
            {
                return NotFound("There are no customers who might be interested in a promo code.");
            }

            // Нигде не определено, поэтому даем промокод пользователю на 7 дней
            promoCode.BeginDate = DateTime.Now.Date;
            promoCode.EndDate = promoCode.BeginDate.AddDays(7);

            // На каждого покупателя создаем свой промокод
            foreach (var customerId in customersIds)
            {
                promoCode.Id = Guid.Empty;
                promoCode.CustomerId = customerId;
                await _promoCodesRepository.AddAsync(promoCode).ConfigureAwait(false);
            }

            return Ok();
        }
    }
}