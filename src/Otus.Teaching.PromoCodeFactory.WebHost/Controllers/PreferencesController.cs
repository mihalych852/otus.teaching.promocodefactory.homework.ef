using System.Collections.Generic;
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
    /// PreferencesController
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController : Controller
    {
        private readonly IRepository<Preference> _dbReference;
        private readonly IMapper _mapper;
        private readonly ILogger<PreferencesController> _logger;

        /// <summary>
        /// PreferencesController construction
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="dbReference"></param>
        public PreferencesController(IRepository<Preference> dbReference, IMapper mapper, ILogger<PreferencesController> logger)
        {
            _dbReference = dbReference;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all preferences
        /// </summary>
        /// <returns>PreferenceResponse</returns>
        [HttpGet]
        public async Task<ActionResult<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _dbReference.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PreferenceResponse>>(preferences);

            return Ok(result);
        }
    }
}
