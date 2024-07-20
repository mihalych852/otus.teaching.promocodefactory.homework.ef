using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class FillDbWithInitialData : IFillDbWithInitialData
    {
        private readonly DbContext _dataContext;

        public FillDbWithInitialData(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Fill()
        {
            _dataContext.AddRange(FakeDataFactory.Roles);
            _dataContext.AddRange(FakeDataFactory.Preferences);
            _dataContext.AddRange(FakeDataFactory.Customers);
            _dataContext.AddRange(FakeDataFactory.Employees);
            _dataContext.SaveChanges();
        }
    }
}
