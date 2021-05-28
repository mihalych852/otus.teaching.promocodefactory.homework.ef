using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public static class EfDbInitializer
    {
        public static void Initialize(DataContext dataContext)
        {
            //dataContext.Database.EnsureCreated();

            if (dataContext.Database.GetPendingMigrations().Any())
            {
                dataContext.Database.Migrate();
            }

            if (dataContext.Employees.Any())
            {
                return;
            }
            
            dataContext.AddRange(FakeDataFactory.Preferences);
            dataContext.SaveChanges();

            dataContext.AddRange(FakeDataFactory.Customers);
            dataContext.SaveChanges();

            dataContext.AddRange(FakeDataFactory.Employees);
            dataContext.SaveChanges();
        }
    }
}
