using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.CustomerMapper
{
    public class CustomerAutoMapper:ICustomerMapper
    {
        public CustomerShortResponse ToShortResponse(Customer customer)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Customer, CustomerShortResponse>());
            var mapper = config.CreateMapper();
            return mapper.Map<CustomerShortResponse>(customer);
        }

        public CustomerResponse ToResponse(Customer customer, IEnumerable<PreferenceShortResponse> preferences, IEnumerable<PromoCodeShortResponse> promocodes)
        {
           var config = new MapperConfiguration(cfg =>
               cfg.CreateMap<PreferenceShortResponse, CustomerResponse>()
                   .ForMember(dest => dest.Preferences,
                       opt => 
                           opt.MapFrom(src => new List<PreferenceShortResponse>(preferences)))
                   .ForMember(dest => dest.PromoCodes,
                       opt =>
                           opt.MapFrom(src => new List<PromoCodeShortResponse>(promocodes))));
           
           var mapper = config.CreateMapper();
           
           return mapper.Map<CustomerResponse>(customer);
        }

        public Customer FromRequestModel(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences, Customer customer = null)
        {
            if (customer == null)
            {
                customer = new Customer()
                {
                    Id = Guid.NewGuid()
                };
            }
            
            var custtomerProference = new List<CustomerPreference>(preferences.Select(p =>
                new CustomerPreference() 
                {
                    CustomerId = customer.Id,
                    PreferenceId = p.Id
                }));
            
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<CreateOrEditCustomerRequest, Customer>()
                    .ForMember(dest => dest.CustomerPreferences,
                        opt => 
                            opt.MapFrom(src => custtomerProference)));
            
            var mapper = config.CreateMapper();
            
            
            return mapper.Map<CreateOrEditCustomerRequest, Customer>(model, customer);
        }
    }
}