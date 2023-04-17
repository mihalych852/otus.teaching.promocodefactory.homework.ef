using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using tus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer
    {
        private static AppDbContext _db;

        public static async Task InitializeAsync(AppDbContext db)
        {
            _db = db;
            await RestoreDbAsync();

            if (!await _db.Employees.AnyAsync())
            {
                await _db.Employees.AddRangeAsync(FakeDataFactory.Employees);
                await _db.SaveChangesAsync();
            }

            if (!await _db.PromoCodes.AnyAsync())
            {
                await _db.PromoCodes.AddRangeAsync(FakeDataFactory.Promocodes);
                await _db.SaveChangesAsync();
            }

            if (!await _db.Customers.AnyAsync())
            {
                await _db.Customers.AddRangeAsync(FakeDataFactory.Customers);
                await _db.SaveChangesAsync();
            }

            if (!await _db.Preferences.AnyAsync())
            {
                await _db.Preferences.AddRangeAsync(FakeDataFactory.Preferences);
                await _db.SaveChangesAsync();
            }

            if (!await _db.Roles.AnyAsync())
            {
                await _db.Roles.AddRangeAsync(FakeDataFactory.Roles);
                await _db.SaveChangesAsync();
            }

            if (!await _db.CustomerPreferences.AnyAsync())
            {
                await _db.CustomerPreferences.AddRangeAsync(FakeDataFactory.CustomerPreferences);
                await _db.SaveChangesAsync();
            }

            await AddRelationsAsync();
        }

        private static async Task RestoreDbAsync()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
        }

        public static async Task AddRelationsAsync()
        {
            var pref = await _db.Preferences.ToListAsync();
            var promo = await _db.PromoCodes.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var emp = await _db.Employees.ToListAsync();
            var cust = await _db.Customers.AsSplitQuery().ToListAsync();

            emp[0].Role = roles[0];
            emp[1].Role = roles[1];

            promo[0].Preference = pref[0];
            promo[1].Preference = pref[2];

            cust[0].Preferences = new List<Preference>() { pref[0] };
            cust[1].Preferences = new List<Preference>() { pref[2] };
            cust[2].Preferences = new List<Preference>() { pref[2] };

            await _db.SaveChangesAsync();

        }
    }
}
