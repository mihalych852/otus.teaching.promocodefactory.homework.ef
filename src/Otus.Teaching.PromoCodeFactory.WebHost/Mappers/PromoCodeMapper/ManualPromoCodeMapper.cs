using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PromoCodeMapper
{
    public class ManualPromoCodeMapper: IPromoCodeMapper
    {
        public PromoCodeShortResponse ToShortResponse(PromoCode promoCode)
        {
            return new PromoCodeShortResponse()
            {
                Id = promoCode.Id,
                BeginDate = promoCode.BeginDate.ToShortDateString(),
                EndDate = promoCode.EndDate.ToShortDateString(),
                Code = promoCode.Code,
                PartnerName = promoCode.PartnerName,
                ServiceInfo = promoCode.ServiceInfo
            };
        }
    }
}