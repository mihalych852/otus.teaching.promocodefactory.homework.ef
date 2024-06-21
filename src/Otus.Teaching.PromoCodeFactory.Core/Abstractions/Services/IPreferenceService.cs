using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services
{
    public interface IPreferenceService
    {
        Task<List<Preference>> GetAllAsync();
    }
}
