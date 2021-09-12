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
            CreateMap<GivePromoCodeRequest, PromoCode>()
                .ForMember(dest => dest.Code,
                    opt => opt.MapFrom(src => src.PromoCode))
                .ForMember(dest => dest.BeginDate,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.EndDate,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.Customer,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.CustomerId,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.PreferenceId,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.Preference,
                    opt => opt.Ignore()) // Ручной мапинг
                .ForMember(dest => dest.PartnerManagerId,
                    opt => opt.Ignore()) // Игнорируем, заполняется не при выдаче
                .ForMember(dest => dest.PartnerManager,
                    opt => opt.Ignore()) // Игнорируем, заполняется не при выдаче
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore()); // Автоматическое создание


            // Preferences
            CreateMap<Preference, PreferenceShortResponse>();
            CreateMap<Preference, PreferenceResponse>()
                .ForMember(dest => dest.Customers,
                    opt => opt.MapFrom(src => src.CustomerPreferences.Select(x => x.Customer)));
        }
    }
}