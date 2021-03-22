using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
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
        private readonly IRepository<Employee> _empRepository;

        public PromocodesController(IRepository<PromoCode> promoRepository, IRepository<Preference> prefRepository, IRepository<Customer> custRepository, IRepository<Employee> empRepository)
        {
            _promoRepository = promoRepository;
            _prefRepository = prefRepository;
            _custRepository = custRepository;
            _empRepository = empRepository;
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

            var preferences = await _prefRepository.GetAllAsync();
            var preference = preferences.Where(x => x.Name == request.Preference).FirstOrDefault();

            if (preference != null)
            {

                var employees = await _empRepository.GetAllAsync();

                var employee = employees.FirstOrDefault();

                if (employee == null)
                    return NotFound();

                var code = new PromoCode()
                {
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    Preference = preference,
                    PartnerManager = employee
                };

                await _promoRepository.AddAsync(code);

                var custs = await _custRepository.GetAllAsync();

                var cc = custs.Where(x => x.Preferences != null && x.Preferences.Any(y => y.Preference.Id == preference.Id)).ToList();

                foreach (var cust in cc)
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