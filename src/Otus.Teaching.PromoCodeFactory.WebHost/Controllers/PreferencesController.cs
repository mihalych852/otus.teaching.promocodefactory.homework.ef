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
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController: ControllerBase
    {
        private readonly IRepository<Preference> _repository;
        private readonly IMapper _mapper;

        public PreferencesController(IRepository<Preference> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список возможных предпочтений клиентов.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenceShortResponse>>> GetPreferences()
        {
            var preferences = await _repository.GetAllAsync().ConfigureAwait(false);
            var preferencesDto = _mapper.Map< IEnumerable<PreferenceShortResponse>>(preferences);
            return Ok(preferencesDto);
        }

        /// <summary>
        /// Получить детальную информацию о предпочтении. Информация включает список клиентов.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceById(Guid id)
        {
            var preference = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            var preferenceDto = _mapper.Map<PreferenceResponse>(preference);
            return Ok(preferenceDto);
        }
    }
}
