using System;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Otus.Teaching.PromoCodeFactory.Core.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<PromoCode> PromoCodes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; }


        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(bc => new { bc.CustomerId, bc.PreferenceId });
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(bc => bc.Customer)
                .WithMany(b => b.Preferences)
                .HasForeignKey(bc => bc.CustomerId);
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(bc => bc.Preference)
                .WithMany()
                .HasForeignKey(bc => bc.PreferenceId);
            modelBuilder.Entity<PromoCode>()
                .HasKey(bc => new { bc.CustomerId });
            modelBuilder.Entity<PromoCode>()
                .HasOne(bc => bc.Customer)
                .WithMany()
                .HasForeignKey(bc => bc.CustomerId);


            //Employee
            modelBuilder.Entity<Employee>().Property(c => c.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(c => c.LastName).HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(c => c.Email).HasMaxLength(200);
            //Customer
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(200);
            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(100);
            //Role
            modelBuilder.Entity<Role>().Property(c => c.Description).HasMaxLength(500);
            modelBuilder.Entity<Role>().Property(c => c.Name).HasMaxLength(100);
            //Preference
            modelBuilder.Entity<Preference>().Property(c => c.Name).HasMaxLength(100);
            //PromoCode
            modelBuilder.Entity<PromoCode>().Property(c => c.Code).HasMaxLength(50);
            modelBuilder.Entity<PromoCode>().Property(c => c.PartnerName).HasMaxLength(200);
            modelBuilder.Entity<PromoCode>().Property(c => c.ServiceInfo).HasMaxLength(200);
        }

    }
}
