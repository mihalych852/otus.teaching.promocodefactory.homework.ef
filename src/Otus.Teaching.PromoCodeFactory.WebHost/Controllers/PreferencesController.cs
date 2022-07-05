using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PreferencesController(IRepository<Preference> preferenceRepository, IUnitOfWork unitOfWork)
        {
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Выгрузить информацию по всем предпочтениям
        /// </summary>
        [ProducesResponseType(typeof(IList<PreferenceResponse>),StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IList<PreferenceResponse>>> GetPreferencesAsync()
        {
            var entities = await _preferenceRepository.GetAllAsync();

            var result = entities
                .Select(item => new PreferenceResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                })
                .ToList();

            return Ok(result);
        }
    }
}