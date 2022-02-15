using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;


namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        //public DbSet<CustomerPreference> customerPreferences { get; set; }


        public DataContext(DbContextOptions<DataContext> dbContextOptions): base(dbContextOptions)
        {
            //Database.EnsureDeleted();
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
            // Employee
            //
            modelBuilder.Entity<Employee>(e =>
                {
                    e.Property(e => e.FirstName).HasMaxLength(50);
                    e.Property(e => e.LastName).HasMaxLength(50);
                    e.Property(e => e.Email).HasMaxLength(50);
                });

            //
            // Role
            //
            modelBuilder.Entity<Role>(e =>
            {
                e.Property(e => e.Name).HasMaxLength(50);
                e.Property(e => e.Description).HasMaxLength(50);
            });

            //
            // Customer
            //
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.PromoCodes)
                .WithOne();
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Preferences)
                .WithOne();
            //modelBuilder.Entity<Customer>(e =>
            //{
            //    e.Property(e => e.FirstName).HasMaxLength(50);
            //    e.Property(e => e.LastName).HasMaxLength(50);
            //    e.Property(e => e.Email).HasMaxLength(50);
            //});

            //
            // CustomerPreference
            //
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Customer)
                .WithMany();
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Preference)
                .WithMany();

            //
            // Preference
            //
            modelBuilder.Entity<Preference>().Property(e => e.Name).HasMaxLength(50);

            //
            // PromoCode
            //
            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.PartnerManager)
                .WithMany();
            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.Preference)
                .WithMany();
            modelBuilder.Entity<PromoCode>(e =>
            {
                e.Property(e => e.Code).HasMaxLength(50);
                e.Property(e => e.ServiceInfo).HasMaxLength(50);
                e.Property(e => e.PartnerName).HasMaxLength(50);
            });

        }

    }
}
