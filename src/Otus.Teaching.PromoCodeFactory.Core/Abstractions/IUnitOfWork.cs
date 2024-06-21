using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Employee> EmployeesRepository { get; }
        IRepository<Role> RolesRepository { get; }
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Preference> PreferencesRepository { get; }
        IRepository<PromoCode> PromoCodeRepository { get; }

        Task SaveChangesAsync();
    }
}
