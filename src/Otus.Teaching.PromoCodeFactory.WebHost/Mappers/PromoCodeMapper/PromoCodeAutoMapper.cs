using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PromoCodeMapper
{
    public class PromoCodeAutoMapper: IPromoCodeMapper
    {
        public PromoCodeShortResponse ToShortResponse(PromoCode promoCode)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<PromoCode, PromoCodeShortResponse>());
            var mapper = config.CreateMapper();
            return mapper.Map<PromoCodeShortResponse>(promoCode);
        }

        public PromoCode FromRequestModel(GivePromoCodeRequest request, Customer customer, Preference preference)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<GivePromoCodeRequest, PromoCode>()
                    .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PromoCode))
                    .ForMember(dest => dest.Customer, 
                        opt => opt.MapFrom(src => customer))
                    .ForMember(dest => dest.Preference,
                        opt => opt.MapFrom(src => preference))
                );
            var mapper = config.CreateMapper();
            return mapper.Map<GivePromoCodeRequest, PromoCode>(request);
        }
    }
}