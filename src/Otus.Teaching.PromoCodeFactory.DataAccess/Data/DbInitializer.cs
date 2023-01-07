using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer
    {
        private static PromocodeFactoryDb _db;
        public static async Task InitializeAsync(PromocodeFactoryDb db, bool restoreDb = false)
        {
            _db = db;
            await RestoreDbAsync(restoreDb);
            await InitializeEmployeesAsync();
            await InitializePromocodesAsync();
            await InitializePreferencesAsync();
            await InitializeCustomersAsync();
            await InitializeRolesAsync();
            await AddRelationsAsync();
        }

        private static async Task RestoreDbAsync(bool restoreDb)
        {
            if(!restoreDb) return;
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
        }

        public static async Task InitializeEmployeesAsync()
        {
            if(await _db.Employees.AnyAsync()) return;
            await _db.Employees.AddRangeAsync(FakeDataFactory.Employees);
            await _db.SaveChangesAsync();
        }

        public static async Task InitializePromocodesAsync()
        {
            if (await _db.PromoCodes.AnyAsync()) return;
            await _db.PromoCodes.AddRangeAsync(FakeDataFactory.Promocodes);
            await _db.SaveChangesAsync();
        }

        public static async Task InitializeCustomersAsync()
        {
            if (await _db.Customers.AnyAsync()) return;
            await _db.Customers.AddRangeAsync(FakeDataFactory.Customers);
            await _db.SaveChangesAsync();
        }

        public static async Task InitializePreferencesAsync()
        {
            if (await _db.Preferences.AnyAsync()) return;
            await _db.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
            await _db.SaveChangesAsync();
        }

        public static async Task InitializeRolesAsync()
        {
            if (await _db.Roles.AnyAsync()) return;
            await _db.Roles.AddRangeAsync(FakeDataFactory.Roles);
            await _db.SaveChangesAsync();
        }

        public static async Task AddRelationsAsync()
        {
            var pref = await _db.Preferences.ToListAsync();
            var promo = await _db.PromoCodes.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var emp = await _db.Employees.ToListAsync();
            var cust = await _db.Customers.ToListAsync();

            #region Employees
            emp[0].Role = roles[0];
            emp[1].Role = roles[1];
            #endregion

            #region Promocodes
            promo[0].Preference = pref[0];
            promo[1].Preference = pref[2];
            #endregion


            #region Customers
            cust[0].Preferences = new List<Preference>() { pref[0], pref[2] };
            cust[0].PromoCodes = new List<PromoCode>() { promo[1] };
            #endregion

            await _db.SaveChangesAsync();

        }
    }
}
