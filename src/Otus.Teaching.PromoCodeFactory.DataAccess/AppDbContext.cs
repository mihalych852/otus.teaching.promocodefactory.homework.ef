using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace tus.Teaching.PromoCodeFactory.DataAccess
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public virtual DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Navigation(e => e.Preferences).AutoInclude();
            modelBuilder.Entity<Customer>().Navigation(e => e.PromoCodes).AutoInclude();
            modelBuilder.Entity<CustomerPreference>().HasKey(nameof(CustomerPreference.PreferenceId), nameof(CustomerPreference.CustomerId));
        }
    }
}
