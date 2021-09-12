using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class PromoCodeDataContextSeed
    {
        public static async Task SeedAsync(PromoCodeDataContext promoCodeDataContext, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!await promoCodeDataContext.Roles.AnyAsync())
                {
                    await promoCodeDataContext.Roles.AddRangeAsync(FakeDataFactory.Roles);
                }

                if (!await promoCodeDataContext.Preferences.AnyAsync())
                {
                    await promoCodeDataContext.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
                }

                if (!await promoCodeDataContext.Customers.AnyAsync())
                {
                    await promoCodeDataContext.Customers.AddRangeAsync(FakeDataFactory.Customers);
                }

                if (!await promoCodeDataContext.Employees.AnyAsync())
                {
                    await promoCodeDataContext.Employees.AddRangeAsync(FakeDataFactory.Employees);
                }

                await promoCodeDataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var log = loggerFactory.CreateLogger<PromoCodeDataContextSeed>();
                log.LogError(ex.Message);
                throw;
            }
        }
    }
}