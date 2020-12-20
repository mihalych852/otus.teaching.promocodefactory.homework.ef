using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper
{
    public class ManualCustomerMapper: ICustomerMapper
    {
        public CustomerShortResponse MapFromCustomer(Customer customer)
        {
            return new CustomerShortResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }

        public CustomerResponse MapFromCustomer(
            Customer customer, IEnumerable<PreferenceShortResponse> preferences, IEnumerable<PromoCodeShortResponse> promocodes)
        {
            return new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Preferences = new List<PreferenceShortResponse>(preferences),
                PromoCodes = new List<PromoCodeShortResponse>(promocodes)
            };
        }
    }
}