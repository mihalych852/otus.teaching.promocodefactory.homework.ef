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
            throw new System.NotImplementedException();
        }
    }
}