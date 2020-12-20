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

        public PreferencesController(
            IRepository<Preference> preferencesRepository,
            IPreferenceMapper preferenceMapper)
        {
            _preferencesRepository = preferencesRepository;
            _preferenceMapper = preferenceMapper;
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
    }
}