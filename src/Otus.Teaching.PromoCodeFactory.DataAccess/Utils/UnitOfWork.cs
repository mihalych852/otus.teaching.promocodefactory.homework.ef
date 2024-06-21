using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IRepository<Employee> employeesRepository,
            IRepository<Role> rolesRepository,
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferencesRepository,
            IRepository<PromoCode> promoCodeRepository
            )
        {
            _dbContext = dbContext;
            EmployeesRepository = employeesRepository;
            RolesRepository = rolesRepository;
            CustomerRepository = customerRepository;
            PreferencesRepository = preferencesRepository;
            PromoCodeRepository = promoCodeRepository;
        }

        public IRepository<Employee> EmployeesRepository { get; }

        public IRepository<Role> RolesRepository { get; }

        public IRepository<Customer> CustomerRepository { get; }

        public IRepository<Preference> PreferencesRepository { get; }

        public IRepository<PromoCode> PromoCodeRepository { get; }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
