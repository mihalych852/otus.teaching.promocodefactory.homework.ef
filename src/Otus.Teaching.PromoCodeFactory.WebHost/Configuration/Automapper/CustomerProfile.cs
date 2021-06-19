using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Configuration.Automapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>();

            CreateMap<CreateOrEditCustomerRequest, Customer>()
                .ForMember(m => m.Preferences,  opt =>
                        opt.MapFrom(d =>
                            (d.PreferenceIds ?? new List<Guid>()).Select(p => new Preference {Id = p}).ToList()));
        }
    }
}