using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Infrastructure.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // DTOs

            // Role controller
            CreateMap<Role, RoleItemResponse>();

            // Customer
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Preferences,
                    opt => opt.MapFrom(src => src.CustomerPreferences.Select(x => x.Preference)));
            CreateMap<CreateOrEditCustomerRequest, Customer>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore()) // Не должно быть при создании и обновлении
                .ForMember(dest => dest.PromoCodes,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.CustomerPreferences,
                    opt => opt.Ignore()); // Ручной мапинг

            // PromoCodes
            CreateMap<PromoCode, PromoCodeShortResponse>();

            // Preferences
            CreateMap<Preference, PreferenceShortResponse>();
            CreateMap<Preference, PreferenceResponse>()
                .ForMember(dest => dest.Customers,
                    opt => opt.MapFrom(src => src.CustomerPreferences.Select(x => x.Customer)));
        }
    }
}