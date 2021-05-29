using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EmployeeRepository: EfRepository<Employee>
    {
        public EmployeeRepository(DataContext dataContext) : base(dataContext)
        {
        }
        
        public override async Task<Employee> GetByIdAsync(Guid id)
        {
            return await DataContext.Employees
                .Include(p => p.Role)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}