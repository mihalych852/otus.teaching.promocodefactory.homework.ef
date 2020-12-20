using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PreferenceMapper
{
    public interface IPreferenceMapper
    {
        public PreferenceResponse MapFromPreference(Preference preference, IEnumerable<CustomerShortResponse> customers);
    }
}