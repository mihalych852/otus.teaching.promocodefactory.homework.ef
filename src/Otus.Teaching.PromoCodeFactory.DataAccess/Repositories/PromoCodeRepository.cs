using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class PromoCodeRepository : EfRepository<PromoCode>
    {
        public PromoCodeRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}