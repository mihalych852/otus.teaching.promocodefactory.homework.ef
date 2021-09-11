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

            // Employee controller
            CreateMap<Employee, EmployeeResponse>();
            CreateMap<Employee, EmployeeShortResponse>();
            //CreateMap<Role, EmployeeRoleItemResponse>();
            //CreateMap<EmployeeCreateRequest, Employee>()
            //    .ForMember(x => x.Roles, opt => opt.Ignore()); // Set from another entity

            //CreateMap<EmployeeUpdateRequest, Employee>()
            //    .ForMember(x => x.Roles, opt => opt.Ignore()); // Set from another entity

            // Role controller
            CreateMap<Role, RoleItemResponse>();

            // Customer
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Preferences,
                    opt => opt.MapFrom(src => src.CustomerPreferences.Select(x => x.Preference)));

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