using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DataContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Preference> Preferences { get; set; }
        
        public DbSet<PromoCode> PromoCodes { get; set; }
    }
}