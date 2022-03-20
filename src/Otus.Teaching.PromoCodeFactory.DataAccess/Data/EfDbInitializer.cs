using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;

//https://stackoverflow.com/questions/60116577/how-to-seed-in-entity-framework-core-3-0

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class EfDbInitializer: IDbInitializer
    {
        private readonly DataContext _dataContext;

        public EfDbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public void InitializeDB()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            _dataContext.Roles.AddRange(FakeDataFactory.Roles);
            _dataContext.Employees.AddRange(FakeDataFactory.Employees);
            _dataContext.PromoCodes.AddRange(FakeDataFactory.PromoCodes);
            //_dataContext.Customers.AddRange(FakeDataFactory.Customers);
            //_dataContext.Preferences.AddRange(FakeDataFactory.Preferences);
            _dataContext.SaveChanges();
        }


    }
}
