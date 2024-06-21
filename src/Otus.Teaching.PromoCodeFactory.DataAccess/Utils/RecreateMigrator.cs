using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Utils
{
    public class RecreateMigrator : IMigrator
    {
        private readonly ApplicationDbContext _context;

        public RecreateMigrator(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
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
