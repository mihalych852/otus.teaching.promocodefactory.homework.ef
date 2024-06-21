using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    ///     Конроллер по работе с препдочтениями клиентов
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferncesController : ControllerBase
    {
        private readonly IPreferenceService _preferenceService;
        private readonly IMapper _mapper;

        public PreferncesController(IPreferenceService preferenceService, IMapper mapper)
        {
            _preferenceService = preferenceService;
            _mapper = mapper;
        }


        /// <summary>
        ///     Получение всех предпочтений
        /// </summary>
        /// <returns>200 + список всех предпочтений в бд</returns>
        [HttpGet]
        public async Task<ActionResult<PrefernceResponse>> GetPreferencesAsync()
        {
            var result = await _preferenceService.GetAllAsync();
            return Ok(_mapper.Map<List<PrefernceResponse>>(result));
        }
    }
}
