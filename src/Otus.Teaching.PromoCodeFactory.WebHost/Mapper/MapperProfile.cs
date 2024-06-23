using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.prefernceResponses, opt => opt.MapFrom(src => src.Preferences));
            CreateMap<PromoCode, PromoCodeShortResponse>();
            CreateMap<Preference, PrefernceResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateOrEditCustomerDto, Customer>();
            CreateMap<CreateOrEditCustomerRequest, CreateOrEditCustomerDto>();
            CreateMap<Preference, PrefernceResponse>();
            CreateMap<PromoCode, PromoCodeShortResponse>()
                .ForMember(dest => dest.BeginDate, opt => opt.MapFrom(src => src.BeginDate.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("dd.MM.yyyy")));
            CreateMap<PromoCodeForCreateDto, PromoCode>();
            CreateMap<GivePromoCodeRequest, PromoCodeForCreateDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PromoCode))
                .ForMember(dest => dest.PreferenceName, opt => opt.MapFrom(src => src.Preference));
        }
    }
}
