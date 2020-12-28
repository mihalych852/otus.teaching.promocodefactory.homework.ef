using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PreferenceMapper;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController:
        ControllerBase
    {
        private readonly IRepository<Preference> _preferencesRepository;

        private readonly IPreferenceMapper _preferenceMapper;
        private readonly ICustomerMapper _customerMapper;

        public PreferencesController(
            IRepository<Preference> preferencesRepository,
            IPreferenceMapper preferenceMapper,
            ICustomerMapper customerMapper)
        {
            _preferencesRepository = preferencesRepository;
            _preferenceMapper = preferenceMapper;
            _customerMapper = customerMapper;
        }
        
        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenceShortResponse>>> GetAllPreferencesAsync()
        {
            var preferences = await _preferencesRepository.GetAllAsync();

            var responce = preferences.Select(_preferenceMapper.ToShortResponse);

            return Ok(responce);
        }

        /// <summary>
        /// Получить предпочтение по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceByIdAsync(Guid id)
        {
            var preference = await _preferencesRepository.GetByIdAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            var customersShortResponse = preference.CustomerPreferences
                .Select(cp => _customerMapper.ToShortResponse(cp.Customer));

            var response = _preferenceMapper.ToResponse(preference, customersShortResponse);
            return Ok(response);
        }
    }
}