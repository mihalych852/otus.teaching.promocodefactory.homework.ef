using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController : ControllerBase
    {
        private readonly IEfRepository<Preference> _preferenceRepository;

        public PreferenceController(IEfRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получение списка предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PrefernceResponse>>> GetCustomersAsync()
        {
            var customers = await _preferenceRepository.GetAllAsync();
            var customersModelList = customers.Select(x =>
                new PrefernceResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            return customersModelList;
        }





    }
}
