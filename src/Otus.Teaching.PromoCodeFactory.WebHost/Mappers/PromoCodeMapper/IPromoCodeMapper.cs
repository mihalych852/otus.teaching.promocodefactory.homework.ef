using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PromoCodeMapper
{
    public interface IPromoCodeMapper
    {
        public PromoCodeShortResponse ToShortResponse(PromoCode promoCode);
    }
}