using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController : Controller
    {
        
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> customerRepository)
        {
            _preferenceRepository = customerRepository;
        }
        
        /// <summary>
        /// Получить предпочтения
        /// </summary>
        [HttpGet]
        public async Task<List<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferencesModelList = preferences.Select(x => 
                new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();

            return preferencesModelList;
        }
    }
}