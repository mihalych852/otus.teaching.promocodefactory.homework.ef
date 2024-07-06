using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        /// <returns>Список предпочтений</returns>
        [HttpGet]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferencesModelList = preferences.Select(x =>
                new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

            return Ok(preferencesModelList);
        }
    }
}
