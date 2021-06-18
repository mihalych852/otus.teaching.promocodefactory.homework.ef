using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public PreferenceController(IRepository<Preference> preferenceRepository, IMapper mapper)
        {
            _preferenceRepository = preferenceRepository ?? throw new ArgumentNullException(nameof(preferenceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <summary>
        /// Возвращает список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PreferenceResponse>> GetAllPreferences()
        {
            return (await _preferenceRepository.GetAllAsync())
                .Select(p => _mapper.Map<PreferenceResponse>(p));
        }
    }
}