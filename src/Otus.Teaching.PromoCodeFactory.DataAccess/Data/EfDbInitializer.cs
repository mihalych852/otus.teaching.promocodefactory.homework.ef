using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Infrastructure;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class EfDbInitializer
        : IDbInitializer
    {
        private readonly DatabaseContext _dataContext;

        public EfDbInitializer(DatabaseContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void InitializeDb()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            _dataContext.AddRange(FakeDataFactory.Employees);
            _dataContext.SaveChanges();

            _dataContext.AddRange(FakeDataFactory.Preferences);
            _dataContext.SaveChanges();

            _dataContext.AddRange(FakeDataFactory.Customers);
            _dataContext.SaveChanges();
        }
    }
}
