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
        private readonly IRepository<PromoCode> _dbPromocode;
        private readonly IRepository<Preference> _dbPreferecnes;
        private readonly IRepository<Customer> _dbCustomers;
        private readonly IMapper _mapper;
        private readonly ILogger<PromocodesController> _logger;

        /// <summary>
        /// PromocodesController
        /// </summary>
        /// <param name="dbPromocode"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public PromocodesController(IRepository<PromoCode> dbPromocode,
            IRepository<Preference> dbPreferecnes,
            IRepository<Customer> dbCustomers,
            IMapper mapper,
            ILogger<PromocodesController> logger)
        {
            _dbPromocode = dbPromocode;
            _dbPreferecnes = dbPreferecnes;
            _dbCustomers = dbCustomers;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns>PromoCodeShortResponse</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _dbPromocode.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PromoCodeShortResponse>>(promocodes);
            return Ok(result);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Так как Preferecne содержит ссылку на коллекцию Customer, невозможно
        /// сделать AutoInclude во FluentAPI для него, а метод Include не доступен т.к.
        /// здесь мы не работаем с DbContext. Репазиторий у нас generic и там применить
        /// Include тоже невозможно, т.к. мы не знаем какой тип прийдёт.
        /// Для решения этой задачи я сделал метод GetEntityWithLoadedSpecificNavigationProperty 
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            _logger.LogInformation("Start GivePromoCodesToCustomersWithPreferenceAsync ...");
            _logger.LogInformation($"Checking for null ...");
            var promoCode = _mapper.Map<PromoCode>(request);

            //TODO Избавиться от захардкоженой строки.
            promoCode.Preference = await _dbPreferecnes.GetEntityWithLoadedSpecificNavigationProperty("Customers", promoCode.Preference.Id);
            
            _logger.LogInformation($"Promocode have the Preference {promoCode.Preference.Id}");

            promoCode.BeginDate = DateTime.Now.Date;

            //TODO Избавить от захаркоженного инта
            promoCode.EndDate = DateTime.Now.Date.AddDays(30);

            _logger.LogInformation($"Promocode BeginDate - {promoCode.BeginDate} EndDate - {promoCode.EndDate}");
            
            await _dbPromocode.CreateAsync(promoCode); 

            var customers = promoCode.Preference.Customers;

            _logger.LogInformation($"Adding promocode for {customers.Count} customers");

            foreach (var customer in customers)
                customer.PromoCodes.Add(promoCode);

            await _dbCustomers.UpdateAsync(customers.ToArray());
            return Ok();
        }
    }
}