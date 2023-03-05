using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        private static IEnumerable<Employee> _employees;

        public static IEnumerable<Employee> Employees => _employees ??= new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                AppliedPromocodesCount = 5,
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                AppliedPromocodesCount = 10
            },
        };

        private static IEnumerable<Role> _role;
        public static IEnumerable<Role> Roles => _role ??= new List<Role>()
        {
            new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        private static IEnumerable<Preference> _preferences;
        public static IEnumerable<Preference> Preferences => _preferences ??= new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        private static IEnumerable<PromoCode> _promoCode;

        public static IEnumerable<PromoCode> Promocodes => _promoCode ??= new List<PromoCode>()
        {
            new PromoCode()
            {
                Code = Guid.NewGuid().ToString(),
                PartnerName = "Company1",

            },
            new PromoCode()
            {
                Code = Guid.NewGuid().ToString(),
                PartnerName = "Company2",
            },
        };

        private static IEnumerable<Customer> _customer;

        public static IEnumerable<Customer> Customers => _customer ??= new List<Customer>()
        {
            new Customer()
            {
                Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                Email = "ivan_sergeev@mail.ru",
                FirstName = "Иван",
                LastName = "Петров",
            },
            new Customer()
            {
                Id = Guid.Parse("73BF1CC8-2B29-4430-89BF-1C0E07378B9A"),
                Email = "sidr@mail.ru",
                FirstName = "Сидр",
                LastName = "Сидоров",
            },
            new Customer()
            {
                Id = Guid.Parse("E123BBC1-4AA0-477E-93FF-29E413F3F809"),
                Email = "lutic@mail.ru",
                FirstName = "Лютик",
                LastName = "Лютикович",
            },
        };
    }
}