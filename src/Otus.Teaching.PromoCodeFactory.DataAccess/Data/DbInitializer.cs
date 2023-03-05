using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer
    {
        private static PromocodeFactoryDb _db;
        private static ILogger<PromocodeFactoryDb> _logger;
        public static async Task InitializeAsync(PromocodeFactoryDb db, ILogger<PromocodeFactoryDb> logger, bool restoreDb = false)
        {
            _db = db;
            _logger = logger;
            _logger.LogInformation("Initializing Db ...");
            await RestoreDbAsync(restoreDb);
            await InitializeEmployeesAsync();
            await InitializePromocodesAsync();
            await InitializePreferencesAsync();
            await InitializeCustomersAsync();
            await InitializeRolesAsync();
            await AddRelationsAsync();
            _logger.LogInformation("Initializing Db has successfully done!");
        }

        private static async Task RestoreDbAsync(bool restoreDb)
        {
            if(!restoreDb) return;
            _logger.LogInformation("Restoring Db ...");
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            _logger.LogInformation("Success");
        }

        public static async Task InitializeEmployeesAsync()
        {
            _logger.LogInformation("Initializing employes ...");
            if (await _db.Employees.AnyAsync())
            {
                _logger.LogInformation("Initializing don't need");
                return;
            }
            await _db.Employees.AddRangeAsync(FakeDataFactory.Employees);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }

        public static async Task InitializePromocodesAsync()
        {
            _logger.LogInformation("Initializing promocodes ...");
            if (await _db.PromoCodes.AnyAsync())
            {
                _logger.LogInformation("Initializing don't need");
                return;
            }
            await _db.PromoCodes.AddRangeAsync(FakeDataFactory.Promocodes);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }

        public static async Task InitializeCustomersAsync()
        {
            _logger.LogInformation("Initializing customers ...");
            if (await _db.Customers.AnyAsync())
            {
                _logger.LogInformation("Initializing don't need");
                return;
            }
            await _db.Customers.AddRangeAsync(FakeDataFactory.Customers);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }

        public static async Task InitializePreferencesAsync()
        {
            _logger.LogInformation("Initializing preferences ...");
            if (await _db.Preferences.AnyAsync())
            {
                _logger.LogInformation("Initializing don't need");
                return;
            }
            await _db.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }

        public static async Task InitializeRolesAsync()
        {
            _logger.LogInformation("Initializing roles ...");
            if (await _db.Roles.AnyAsync())
            {
                _logger.LogInformation("Initializing don't need");
                return;
            }
            await _db.Roles.AddRangeAsync(FakeDataFactory.Roles);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }

        public static async Task AddRelationsAsync()
        {
            _logger.LogInformation("Filling navigation property based collections ...");

            var pref = await _db.Preferences.ToListAsync();
            var promo = await _db.PromoCodes.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var emp = await _db.Employees.ToListAsync();
            var cust = await _db.Customers.AsSplitQuery().ToListAsync();

            #region Employees
            _logger.LogInformation("Filling employee's Roles ...");
            emp[0].Role = roles[0];
            emp[1].Role = roles[1];
            _logger.LogInformation("Success");
            #endregion

            #region Promocodes
            _logger.LogInformation("Filling promocode's Preferences");
            promo[0].Preference = pref[0];
            promo[1].Preference = pref[2];
            _logger.LogInformation("Success");
            #endregion

            #region Customers
            _logger.LogInformation("Filling customer's Preferences ...");
            cust[0].Preferences = new List<Preference>() { pref[0] };
            cust[1].Preferences = new List<Preference>() { pref[2] };
            cust[2].Preferences = new List<Preference>() { pref[2] };
            _logger.LogInformation("Success");
            #endregion

            _logger.LogInformation("Saving changes...");
            await _db.SaveChangesAsync();
            _logger.LogInformation("Success");
        }
    }
}
