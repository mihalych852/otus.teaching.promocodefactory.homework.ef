using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Предпочтения
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PreferenceController: ControllerBase {
    private readonly IRepository<Preference> _repositoryPreference;

    public PreferenceController(IRepository<Preference> repositoryPreference) {
        _repositoryPreference = repositoryPreference;
    }

    /// <summary>
    /// Получить список всех предпочтений
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<PreferenceResponse>> GetPreferenceAsync() {
        var preference = await _repositoryPreference.GetAllAsync();
        return preference.Select(x => new PreferenceResponse {
            Id = x.Id,
            Name = x.Name
        });
    }
}