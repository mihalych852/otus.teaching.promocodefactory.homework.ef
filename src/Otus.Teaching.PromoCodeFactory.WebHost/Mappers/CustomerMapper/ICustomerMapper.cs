using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper
{
    public interface ICustomerMapper
    {
        public CustomerShortResponse ToShortResponse(Customer customer);

        public CustomerResponse ToResponse(
            Customer customer, IEnumerable<PreferenceShortResponse> preferences, 
            IEnumerable<PromoCodeShortResponse> promocodes);
    }
}