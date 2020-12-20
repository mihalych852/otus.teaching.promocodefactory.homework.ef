using System;
using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper
{
    public class ManualCustomerMapper: ICustomerMapper
    {
        public CustomerShortResponse ToShortResponse(Customer customer)
        {
            return new CustomerShortResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }

        public CustomerResponse ToResponse(
            Customer customer, 
            IEnumerable<PreferenceShortResponse> preferences, IEnumerable<PromoCodeShortResponse> promocodes)
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

        public Customer FromRequestModel(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences, Customer customer = null)
        {
            if (customer == null)
            {
                customer = new Customer();
                customer.Id = Guid.NewGuid();
            }
            
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.CustomerPreferences = new List<CustomerPreference>(
                preferences
                    .Select(p=> new CustomerPreference()
                    {
                        CustomerId = customer.Id,
                        PreferenceId = p.Id
                    }));
            
            return customer;
        }
    }
}