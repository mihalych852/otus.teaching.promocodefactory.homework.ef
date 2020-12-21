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
                {
                    cfg.CreateMap<GivePromoCodeRequest, PromoCode>()
                        .ForMember(dest => dest.Code,
                            opt =>
                                opt.MapFrom(src => src.PromoCode))
                        .ForMember(dst => dst.Customer,
                            opt => opt.Ignore())
                        .ForMember(dst => dst.Preference,
                            opt => opt.Ignore());
                    cfg.CreateMap<Customer, PromoCode>()
                        .ForMember(dst => dst.Customer,
                            opt =>
                                opt.MapFrom(src => src))
                        .ForAllOtherMembers(opt => opt.Ignore());
                    cfg.CreateMap<Preference, PromoCode>()
                        .ForMember(dst => dst.Preference,
                            opt =>
                                opt.MapFrom(src => src))
                        .ForAllOtherMembers(opt => opt.Ignore());
                });
            
            var mapper = config.CreateMapper();
            var promoCode = mapper.Map<PromoCode>(request);
            mapper.Map(customer, promoCode);
            mapper.Map(preference, promoCode);
            return promoCode;
        }
    }
}