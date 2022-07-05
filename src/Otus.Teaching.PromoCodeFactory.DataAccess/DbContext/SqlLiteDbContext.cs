using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.DbContext
{
    public class SqlLiteDbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
    {
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Preference> Preferences { get; set; }
        
        public DbSet<PromoCode> PromoCodes { get; set; }

        public SqlLiteDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .HasOne(item => item.Role)
                .WithMany(item => item.Employees)
                .HasForeignKey(emp => emp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<PromoCode>()
                .HasOne(item => item.PartnerManager)
                .WithMany(item => item.Promocodes)
                .HasForeignKey(item => item.PartnerManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<PromoCode>()
                .HasOne(item => item.Customer)
                .WithMany(item => item.Promocodes)
                .HasForeignKey(item => item.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<PromoCode>()
                .HasOne(item => item.Preference)
                .WithMany(item => item.Promocodes)
                .HasForeignKey(item => item.PreferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CustomerPreference>()
                .HasKey(item => new { item.CustomerId, item.PreferenceId });

            modelBuilder
                .Entity<CustomerPreference>()
                .HasOne(item => item.Customer)
                .WithMany(item => item.CustomerPreferences)
                .HasForeignKey(item => item.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<CustomerPreference>()
                .HasOne(item => item.Preference)
                .WithMany(item => item.CustomerPreferences)
                .HasForeignKey(item => item.PreferenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Customer>()
                .HasIndex(property => property.Email)
                .IsUnique();

            modelBuilder
                .Entity<Role>()
                .HasData(FakeDataFactory.Roles);

            modelBuilder
                .Entity<Employee>()
                .HasData(FakeDataFactory.Employees);

            modelBuilder
                .Entity<Customer>()
                .HasData(FakeDataFactory.Customers);

            modelBuilder
                .Entity<Preference>()
                .HasData(FakeDataFactory.Preferences);

            modelBuilder
                .Entity<CustomerPreference>()
                .HasData(FakeDataFactory.CustomerPreferences);
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(cancellationToken);
        }
    }
}