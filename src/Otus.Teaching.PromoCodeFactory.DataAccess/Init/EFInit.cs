using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

using System.Linq;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Init
{
    public class EFInit : IEFInit
    {
        private readonly EFDataContext _dataContext;

        public EFInit(EFDataContext dataContext) {
            _dataContext = dataContext;
        }

        public void Init()
        {
            if (!_dataContext.Database.EnsureCreated())
            {
                _dataContext.Customers.RemoveRange(_dataContext.Customers);
                _dataContext.Preferences.RemoveRange(_dataContext.Preferences);
                _dataContext.PromoCodes.RemoveRange(_dataContext.PromoCodes);
                _dataContext.SaveChanges();

                _dataContext.Employees.RemoveRange(_dataContext.Employees);
                _dataContext.Roles.RemoveRange(_dataContext.Roles);
                _dataContext.SaveChanges();
            }


            _dataContext.Roles.AddRange(FakeDataFactory.Roles);
            _dataContext.Preferences.AddRange(FakeDataFactory.Preferences);
            _dataContext.Customers.AddRange(FakeDataFactory.Customers);
            _dataContext.SaveChanges();
            _dataContext.Employees.AddRange(FakeDataFactory.Employees.Select(e =>
            {
                e.Role = _dataContext.Roles.Single(r => r.Id == e.Role.Id);    
                return e;
            }));
            _dataContext.SaveChanges();

        }
    }
}
