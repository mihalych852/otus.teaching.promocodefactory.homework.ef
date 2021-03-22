using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public Context()
        {
            
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role
            modelBuilder.Entity<Role>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(c => c.Name)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(c => c.Description)
                .HasMaxLength(256);


            // Employee
            modelBuilder.Entity<Employee>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Employee>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(c => c.FirstName)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Employee>()
               .Property(c => c.LastName)
               .HasMaxLength(256)
               .IsRequired();

            modelBuilder.Entity<Employee>()
               .Property(c => c.Email)
               .HasMaxLength(256)
               .IsRequired();

            modelBuilder.Entity<Employee>()
               .Property(c => c.AppliedPromocodesCount);

            //modelBuilder.Entity<Employee>()
            //    .HasOne(bc => bc.Role)
            //    .WithMany()
            //    .HasForeignKey(bc => bc.RoleId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Customer
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Customer>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Customer>()
               .Property(c => c.LastName)
               .HasMaxLength(256)
               .IsRequired();

            modelBuilder.Entity<Customer>()
               .Property(c => c.Email)
               .HasMaxLength(256)
               .IsRequired();

            // Preference
            modelBuilder.Entity<Preference>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Preference>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Preference>()
                .Property(c => c.Name)
                .HasMaxLength(256)
                .IsRequired();

            // PromoCode
            modelBuilder.Entity<PromoCode>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PromoCode>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
                .Property(c => c.Code)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
               .Property(c => c.ServiceInfo)
               .HasMaxLength(256)
               .IsRequired();

            modelBuilder.Entity<PromoCode>()
               .Property(c => c.PartnerName)
               .HasMaxLength(256)
               .IsRequired();

            //modelBuilder.Entity<PromoCode>()
            //    .HasOne(bc => bc.PartnerManager)
            //    .WithMany()
            //    .HasForeignKey(bc => bc.PartnerManagerId);

            //modelBuilder.Entity<PromoCode>()
            //    .HasOne(bc => bc.Preference)
            //    .WithMany()
            //    .HasForeignKey(bc => bc.PreferenceId);

            // CustomerPreference
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(bc => new { bc.CustomerId, bc.PreferenceId });

            //modelBuilder.Entity<CustomerPreference>()
            //    .HasOne(bc => bc.Customer)
            //    .WithMany(b => b.Preferences)
            //    .HasForeignKey(bc => bc.CustomerId);

            //modelBuilder.Entity<CustomerPreference>()
            //    .HasOne(bc => bc.Preference)
            //    .WithMany()
            //    .HasForeignKey(bc => bc.PreferenceId);
        }
    }
}