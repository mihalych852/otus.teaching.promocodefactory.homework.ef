using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Utils
{
    public class Migrator : IMigrator
    {
        private readonly ApplicationDbContext _context;

        public Migrator(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            var isCreated = _context.Database.EnsureCreated();
            _context.Database.Migrate();

            if(isCreated)
                FillDbViaFakeData();
        }

        private void FillDbViaFakeData()
        {
            _context.AddRange(FakeDataFactory.Employees);
            _context.AddRange(FakeDataFactory.Customers);
            _context.SaveChanges();
        }
    }
}
