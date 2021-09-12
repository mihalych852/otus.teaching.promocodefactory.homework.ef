using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories.EntityFrameworkRepositories
{
    public class CustomersEntityFrameworkRepository : EntityFrameworkRepository<Customer>, ICustomersRepository
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

        public async Task<IEnumerable<Guid>> GetCustomersIdsByPreferenceId(Guid preferenceId)
        {
            var result = await DbContext.CustomerPreferences
                .Where(x => x.PreferenceId == preferenceId)
                .Select(x => x.CustomerId)
                .ToListAsync();

            return result;
        }
    }
}