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
        public DbSet<Preference> Preferences{ get; set; }
        public DbSet<PromoCode> PromoCodes{ get; set; }
        //public DbSet<CustomerPreference> customerPreferences { get; set; }


        public DataContext(DbContextOptions<DbContext> dbContextOptions): base(dbContextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithOne();
            //modelBuilder.Entity<Customer>()
            //   .HasMany(c => c.Preferences)
            //   .WithMany(c => c.Customers)
            //   .Map(cp =>
            //   {
            //       cp.MapLeftKey("CustomerRefId");
            //       cp.MapRightKey("PreferenceRefId");
            //       cp.ToTable("CustomerPreference");
            //   });
            //modelBuilder.Entity<Customer>()
            // .HasMany(c => c.Preferences)
            // .WithMany(p => p.Customers)
            // .UsingEntity(cp => cp.ToTable("CustomerPreference"));

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Customer)
                .WithMany();
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Preference)
                .WithMany();

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.PromoCodes)
                .WithOne();
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Preferences)
                .WithOne();

            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.PartnerManager)
                .WithMany();
            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.Preference)
                .WithMany();
        }

    }
}
