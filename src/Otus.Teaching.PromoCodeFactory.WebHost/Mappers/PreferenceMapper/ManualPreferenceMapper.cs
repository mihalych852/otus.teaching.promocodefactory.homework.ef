using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PreferenceMapper
{
    public class ManualPreferenceMapper: IPreferenceMapper
    {
        public PreferenceResponse ToResponse(Preference preference, IEnumerable<CustomerShortResponse> customers)
        {
            return new PreferenceResponse()
            {
                Id = preference.Id,
                Name = preference.Name,
                Customers = new List<CustomerShortResponse>(customers)
            };
        }

        public PreferenceShortResponse ToShortResponse(Preference preference)
        {
            return new PreferenceShortResponse()
            {
                Id = preference.Id,
                Name = preference.Name
            };
        }
    }
}