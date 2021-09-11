using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories.EntityFrameworkRepositories
{
    public class CustomersEntityFrameworkRepository : EntityFrameworkRepository<Customer>
    {
        public CustomersEntityFrameworkRepository(PromoCodeDataContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Customer> GetByIdAsync(Guid id)
        {
            var item = await DbContext.Customers
                .Where(x => x.Id == id)
                .Include(x => x.PromoCodes)
                .Include(x => x.CustomerPreferences)
                .ThenInclude(x => x.Preference)
                .FirstOrDefaultAsync();

            return item;
        }
    }
}