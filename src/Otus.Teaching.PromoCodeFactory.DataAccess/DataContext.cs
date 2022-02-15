using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;


namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext: DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        //public DbSet<CustomerPreference> customerPreferences { get; set; }


        public DataContext(DbContextOptions<DataContext> dbContextOptions): base(dbContextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            //
            //all string column 
            //
            //modelBuilder.Properties<string>().Configure(p => p.IsMaxLength(50));

            //
            // Role
            //
            modelBuilder.Entity<Role>(e =>
            {
                e.Property(r => r.Name).HasMaxLength(50);
                e.Property(r => r.Description).HasMaxLength(50);
                e.HasData(FakeDataFactory.Roles);
            });
            modelBuilder.Entity<Role>().HasData(new InMemoryRepository<Role>(FakeDataFactory.Roles));

            //
            // Employee
            //
            modelBuilder.Entity<Employee>(e =>
                {
                    e.HasOne(x => x.Role);
                    e.Property(x => x.FirstName).HasMaxLength(50);
                    e.Property(x => x.LastName).HasMaxLength(50);
                    e.Property(x => x.Email).HasMaxLength(50);
                    e.HasData(FakeDataFactory.Employees);
                });

            //
            // Customer
            //
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasMany(x => x.PromoCodes).WithOne();
                //e.HasMany(x => x.Preferences).WithOne();
                e.Property(x => x.FirstName).HasMaxLength(50);
                e.Property(x => x.LastName).HasMaxLength(50);
                e.Property(x => x.Email).HasMaxLength(50);
                //e.HasData(FakeDataFactory.Customers);
            });

            //
            // CustomerPreference
            //
            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Customers)
                .WithMany(c => c.Preferences)
                .UsingEntity(j => j.ToTable("CustomerPreference"));

            //
            // Preference
            //
            modelBuilder.Entity<Preference>(e =>
            {
                e.Property(e => e.Name).HasMaxLength(50);
                e.HasData(FakeDataFactory.Preferences);
            });

            //
            // PromoCode
            //
            modelBuilder.Entity<PromoCode>(e =>
            {
                e.HasOne(p => p.PartnerManager).WithMany();
                e.HasOne(p => p.Preference).WithMany();
                e.Property(p => p.Code).HasMaxLength(50);
                e.Property(p => p.ServiceInfo).HasMaxLength(50);
                e.Property(p => p.PartnerName).HasMaxLength(50);
                //e.HasData(FakeDataFactory.PromoCodes);
            });


        }
        //public void Dispose()
        //{
        //    CloseConnection();
        //    testDb.Dispose();
        //}
    }
}
