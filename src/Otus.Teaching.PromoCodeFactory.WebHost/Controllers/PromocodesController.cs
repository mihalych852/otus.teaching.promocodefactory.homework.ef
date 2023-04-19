using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        private IMapper _mapper;
        private readonly IRepository<PromoCode> _repo;
        private readonly IRepository<PromoCode> _repoPromocode;
        private readonly IRepository<Customer> _repoCustomer;
        private readonly IRepository<Preference> _repoPreference;

        public PromocodesController(IMapper mapper, IRepository<PromoCode> repoPromocode, IRepository<Customer> repoCustomer, IRepository<Preference> repoPreference)
        {
            _mapper = mapper;
             _repoCustomer = repoCustomer;
            _repoPromocode = repoPromocode;
            _repoPreference = repoPreference;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var customers = await _repo.GetAllAsync();
            var customersDto = _mapper.Map<IEnumerable<PromoCodeShortResponse>>(customers);
            return Ok(customersDto);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {

            var promoCode = _mapper.Map<PromoCode>(request);

            promoCode.Preference = await _repoPreference.GetEntityWithLoadedSpecificNavigationProperty("Customers", promoCode.Preference.Id);
            promoCode.BeginDate = DateTime.Now.Date;
            promoCode.EndDate = DateTime.Now.Date.AddDays(30);

            await _repoPromocode.CreateAsync(promoCode);

            var customers = promoCode.Preference.Customers;

            foreach (var customer in customers)
            {
                customer.PromoCodes.Add(promoCode);
                await _repoCustomer.UpdateAsync(customer);
            }
            
            return Ok();

        }


    }
}