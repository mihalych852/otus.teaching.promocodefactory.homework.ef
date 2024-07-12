using System;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;


namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasKey(e => e.Id);
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Preference>().HasKey(e => e.Id);
            modelBuilder.Entity<Customer>().HasKey(e => e.Id);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Promocodes)
                .WithOne(p => p.Customer);

            modelBuilder.Entity<Preference>()
                    .HasMany(p => p.Customers)
                    .WithMany(c => c.Preferences);

            modelBuilder.Entity<Customer>().Property(e => e.Email).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(e => e.LastName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(e => e.FirstName).HasMaxLength(100);

            modelBuilder.Entity<PromoCode>().Property(e => e.ServiceInfo).HasMaxLength(100);
            modelBuilder.Entity<PromoCode>().Property(e => e.PartnerName).HasMaxLength(200);
            modelBuilder.Entity<PromoCode>().Property(e => e.Code).HasMaxLength(10);

            modelBuilder.Entity<Preference>().Property(e => e.Name).HasMaxLength(100);

            modelBuilder.Entity<Role>().Property(e => e.Name).HasMaxLength(100);
            modelBuilder.Entity<Role>().Property(e => e.Description).HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(e => e.Email).HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(e => e.LastName).HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasMaxLength(100);

            //modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);
            //modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees);
            //modelBuilder.Entity<Preference>().HasData(FakeDataFactory.Preferences); 
            //modelBuilder.Entity<Customer>().HasData(FakeDataFactory.Customers);
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }
    }
}
