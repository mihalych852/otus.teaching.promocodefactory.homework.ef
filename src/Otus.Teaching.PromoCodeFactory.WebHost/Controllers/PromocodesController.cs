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
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<Customer> customerRepository,
            IMapper mapper)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PromoCodeShortResponse>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodesRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PromoCodeShortResponse>>(promoCodes);

        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            var preferences = await _preferenceRepository.GetAllAsync();
            var preference = preferences.SingleOrDefault(p =>
                p.Name.Equals(request.Preference, StringComparison.OrdinalIgnoreCase));

            if (preference == null)
                return NotFound();

            var customers = await _customerRepository.GetAllAsync();
            var customerWithPreference = customers
                .Where(c => c.Preferences.Any(p => p.Id == preference.Id));
            foreach (var customer in customerWithPreference)
            {
                var promoCodeId = Guid.NewGuid();

                var promoCode = new PromoCode
                {
                    ServiceInfo = request.ServiceInfo,
                    Preference = preference,
                    BeginDate = DateTime.Now,
                    Code = request.PromoCode,
                    EndDate = DateTime.Now.AddMonths(1),
                    PartnerName = request.PartnerName,
                    Id = promoCodeId
                };

                if (!await _promoCodesRepository.AddAsync(promoCode))
                    return BadRequest();

                customer.PromoCodes.Add(promoCode);
                if (!await _customerRepository.UpdateAsync(customer)) return BadRequest();
            }

            return Ok();

        }
    }
}