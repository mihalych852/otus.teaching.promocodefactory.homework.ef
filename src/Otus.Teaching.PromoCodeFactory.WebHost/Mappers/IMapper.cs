using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers
{
    public interface IMapper
    {
        public CustomerShortResponse MapFromCustomer(Customer customer);
    }
}