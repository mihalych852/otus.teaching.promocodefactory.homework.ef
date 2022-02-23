using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

            _dataContext.Roles.AddRange(FakeDataFactory.Roles);
            _dataContext.Employees.AddRange(FakeDataFactory.Employees);
            _dataContext.PromoCodes.AddRange(FakeDataFactory.PromoCodes);
            //_dataContext.Preferences.AddRange(FakeDataFactory.Preferences);
            //_dataContext.Customers.AddRange(FakeDataFactory.Customers);
            _dataContext.SaveChanges();
        }


    }
}
