using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPreferencesRepository : IRepository<Preference>
    {
        Task<Preference> GetPreferenceByName(string preferenceName);
    }
}