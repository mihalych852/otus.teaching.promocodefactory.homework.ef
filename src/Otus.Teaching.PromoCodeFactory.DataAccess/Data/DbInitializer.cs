using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Context;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DbInitializer
    {
        private static PromocodeFactoryDb _db;
        public static void Initialize(PromocodeFactoryDb db, bool restoreDb = false)
        {
            _db = db;
            RestoreDb(restoreDb);
            InitializeEmployees();
            InitializePromocodes();
            InitializePreferences();
            InitializeCustomers();
            InitializeRoles();
            AddRelations();
        }

        private static void RestoreDb(bool restoreDb)
        {
            if(!restoreDb) return;
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        public static void InitializeEmployees()
        {
            if(_db.Employees.Any()) return;
            _db.Employees.AddRange(FakeDataFactory.Employees);
            _db.SaveChanges();
        }

        public static void InitializePromocodes()
        {
            if (_db.PromoCodes.Any()) return;
            _db.PromoCodes.AddRange(FakeDataFactory.Promocodes);
            _db.SaveChanges();
        }

        public static void InitializeCustomers()
        {
            if (_db.Customers.Any()) return;
            _db.Customers.AddRange(FakeDataFactory.Customers);
            _db.SaveChanges();
        }

        public static void InitializePreferences()
        {
            if (_db.Preferences.Any()) return;
            _db.Preferences.AddRange(FakeDataFactory.Preferences);
            _db.SaveChanges();
        }

        public static void InitializeRoles()
        {
            if (_db.Roles.Any()) return;
            _db.Roles.AddRange(FakeDataFactory.Roles);
            _db.SaveChanges();
        }

        public static void AddRelations()
        {
            var pref = _db.Preferences.ToList();
            var promo = _db.PromoCodes.ToList();
            var roles = _db.Roles.ToList();
            var emp = _db.Employees.ToList();
            var cust = _db.Customers.ToList();


            #region Employees
            emp[0].Role = roles[0];
            emp[1].Role = roles[1];
            _db.SaveChanges();
            #endregion

            #region Promocodes
            promo[0].Preference = pref[0];
            promo[1].Preference = pref[2];
            _db.SaveChanges();
            #endregion


            #region Customers

            cust[0].Preferences = new List<Preference>() { pref[0], pref[2] };
            cust[0].PromoCodes = new List<PromoCode>() { promo[1] };
            _db.SaveChanges();
            #endregion

        }
    }
}
