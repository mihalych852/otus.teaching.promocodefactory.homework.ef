using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private IMapper _mapper;
        private IRepository<Preference> _repo;

        public PreferenceController(IMapper mapper, IRepository<Preference> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Получение списка предпочтений
        /// </summary>
        /// <returns>Возвращает список предпочтений</returns>
        [HttpGet]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceAsync()
        {
            var preferences = await _repo.GetAllAsync();

            var preferencesModelList = preferences.Select(x =>
                 new RoleItemResponse()
                 {
                     Id = x.Id,
                     Name = x.Name
                 }).AsEnumerable();

            return Ok(preferencesModelList);
        }
    }
}
