using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : EfRepository<Customer>
    {
        public CustomerRepository(DataContext dataContext) : base(dataContext)
        {
        }
        
        public override async Task<Customer> GetByIdAsync(Guid id)
        {
            return await DataContext.Customers
                .Include(p => p.CustomerPreferences)
                .Include(p => p.PromoCodes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}