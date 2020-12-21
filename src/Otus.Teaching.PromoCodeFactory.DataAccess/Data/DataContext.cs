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
        
        public DataContext()
        {
            
        }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                optionsBuilder.UseSqlite("Filename=PromoCodeFactoryDb.sqlite");
            }
            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create composite key
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(cp => new {cp.CustomerId, cp.PreferenceId});
            
            //one-to-many CustomerPreference-Customer
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(cp => cp.CustomerId);

            //one-to-many CustomerPreference-Preference
            modelBuilder.Entity<Preference>()
                .HasMany(p => p.CustomerPreferences)
                .WithOne(cp => cp.Preference)
                .HasForeignKey(cp => cp.PreferenceId);

            //каскадное удаление промокодов при удалении кастомера
            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.PromoCodes)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}