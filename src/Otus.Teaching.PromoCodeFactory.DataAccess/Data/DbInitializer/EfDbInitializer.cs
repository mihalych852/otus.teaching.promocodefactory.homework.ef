using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data.DbInitializer
{
    public class EfDbInitializer:IDbInitializer
    {
        private readonly DataContext _dataContext;

        public EfDbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public void InitializeDb()
        {
            //_dataContext.Database.EnsureDeleted();
            //_dataContext.Database.EnsureCreated();
            
            _dataContext.Database.Migrate();

            if (!_dataContext.Preferences.Any())
            {
                _dataContext.AddRange(FakeDataFactory.Preferences);
                _dataContext.SaveChanges();
            }

            if (!_dataContext.Customers.Any())
            {
                _dataContext.AddRange(FakeDataFactory.Customers);
                _dataContext.SaveChanges();
            }

            if (!_dataContext.Employees.Any())
            {
                _dataContext.AddRange(FakeDataFactory.Employees);
                _dataContext.SaveChanges();
            }

            

            
        }
    }
}