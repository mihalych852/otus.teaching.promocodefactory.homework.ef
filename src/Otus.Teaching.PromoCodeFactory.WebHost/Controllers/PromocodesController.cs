using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    ///     Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IPromoCodeService _service;
        private readonly IMapper _mapper;

        public PromocodesController(IPromoCodeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        ///     Получить все промокоды
        /// </summary>
        /// <returns>200 + список всех промокодов</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var result = await _service.GetAll();
            return Ok(_mapper.Map<List<PromoCodeShortResponse>>(result));
        }

        /// <summary>
        ///     Создать промокоды и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <param name="request">Модель нового промокода, который нужно отдать покупателям</param>
        /// <returns>
        ///     200 - если удалось добавить новый промокод,
        ///     404 - не удалось найти либо предпочтение, либо покупателей, у которых есть предпочтение
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var result = await _service.AddPromoCodeToCustomerViaPreference(_mapper.Map<PromoCodeForCreateDto>(request));
            if (result)
                return Ok();
            return NotFound();
        }
    }
}