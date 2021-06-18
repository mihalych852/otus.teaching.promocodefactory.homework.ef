using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public PromocodesController(
            IRepository<PromoCode> promoCodeRepository, 
            IRepository<Preference> preferenceRepository, 
            IRepository<Customer> customerRepository, 
            IMapper mapper)
        {
            _promoCodeRepository = promoCodeRepository ?? throw new ArgumentNullException(nameof(promoCodeRepository));
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper;
        }
        
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            return (await _promoCodeRepository.GetAllAsync())
                .Select(p => _mapper.Map<PromoCodeShortResponse>(p))
                .ToList();
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PromoCodeShortResponse>> GivePromoCodesToCustomersWithPreferenceAsync([Required][FromBody] GivePromoCodeRequest request)
        {
            var preference = (await _preferenceRepository.GetAsync(p => p.Name == request.Preference, 
                    nameof(Preference.Customers)))
                .FirstOrDefault();

            if (preference == null)
                return NotFound();

            var promoCode = _mapper.Map<PromoCode>(request);
            promoCode.Preference = preference;

            await _promoCodeRepository.CreateAsync(promoCode);

            if (preference.Customers?.Any() ?? false)
            {
                foreach (var customer in preference.Customers)
                {
                    customer.PromoCodes ??= new List<PromoCode>(1);
                    customer.PromoCodes.Add(promoCode);
                    await _customerRepository.UpdateAsync(customer);
                }
            }

            return _mapper.Map<PromoCodeShortResponse>(promoCode);
        }
    }
}