using Microsoft.EntityFrameworkCore.Metadata;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer
    {
        private readonly PromocodeFactoryDb _db;

        public DbInitializer(PromocodeFactoryDb db)
        {
            _db = db;
        }

        public void Initialize(bool restoreDb)
        {
            RestoreDb(restoreDb);
            InitializeEmployees();
            InitializePromocodes();
            InitializeCustomers();
            InitializePreferences();
        }

        private void RestoreDb(bool restoreDb)
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        public void InitializeEmployees()
        {
            _db.Employees.AddRange(FakeDataFactory.Employees);
            _db.SaveChanges();
        }

        public void InitializePromocodes()
        {
            _db.PromoCodes.AddRange(FakeDataFactory.Promocodes);
            _db.SaveChanges();
        }

        public void InitializeCustomers()
        {
            _db.Customers.AddRange(FakeDataFactory.Customers);
            _db.SaveChanges();
        }

        public void InitializePreferences()
        {
            _db.Preferences.AddRange(FakeDataFactory.Preferences);
            _db.SaveChanges();
        }
    }
}
