using System;
using System.Collections.Generic;
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

        private IMapper _mapper;
        private IRepository<PromoCode> _repo;
        public PromocodesController(IMapper mapper, IRepository<PromoCode> repo)
        {
            _mapper = mapper;
            _repo = repo;
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
            var model = _mapper.Map<GivePromoCodeRequest, PromoCode>(request);
            var result = await _repo.CreateAsync(model);
            return Ok(result);
        }


    }
}