using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers
{
    public class ManualMapper: IMapper
    {
        public CustomerShortResponse MapFromCustomer(Customer customer)
        {
            return new CustomerShortResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }
    }
}