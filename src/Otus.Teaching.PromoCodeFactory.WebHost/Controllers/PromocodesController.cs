using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PromoCodeMapper;
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
        private readonly IRepository<PromoCode> _promocodeRepository;

        private readonly IPromoCodeMapper _promoCodeMapper;

        public PromocodesController(
            IRepository<Preference> preferenceRepository,
            IRepository<PromoCode> promocodeRepository,
            IPromoCodeMapper promoCodeMapper)
        {
            _preferenceRepository = preferenceRepository;
            _promocodeRepository = promocodeRepository;

            _promoCodeMapper = promoCodeMapper;
        }
        
        
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promocodeRepository.GetAllAsync();
            var response = promocodes.Select(_promoCodeMapper.ToShortResponse);

            return Ok(response);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //получаем объект предпочтения
            var preference = (await _preferenceRepository.GetAllAsync())
                .FirstOrDefault(p=> p.Name == request.Preference);
            
            if (preference == null)
            {
                return NotFound();
            }

            //Список пользователей с полученным предпочтением
            var customers = preference.CustomerPreferences.Select(cp => cp.Customer);
            
            foreach (var customer in customers)
            {
                var promocode = _promoCodeMapper.FromRequestModel(request, customer, preference);
                await _promocodeRepository.AddAsync(promocode);
            }

            return Ok();
        }
    }
}