using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Context
{
    public class PromocodeFactoryDb : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public PromocodeFactoryDb(DbContextOptions<PromocodeFactoryDb> options) : base(options)
        { }
    }
}
