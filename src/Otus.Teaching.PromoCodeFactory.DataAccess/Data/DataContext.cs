using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DataContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}