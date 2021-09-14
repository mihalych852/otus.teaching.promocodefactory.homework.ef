using Microsoft.EntityFrameworkCore;

using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Util;

using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class EFDataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Preference> Preferences { get; set; }

        public EFDataContext() { }
        public EFDataContext(DbContextOptions<EFDataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasValueGenerator<IdGenerator>();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Navigation(e => e.Role).AutoInclude();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasValueGenerator<IdGenerator>();
                entity.HasMany<Employee>()
                    .WithOne(e => e.Role);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Id).HasValueGenerator<IdGenerator>();
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(30);
                entity.Property(e => e.LastName).HasMaxLength(30);

                entity.Navigation(e =>  e.Preferences).AutoInclude();
                entity.Navigation(e => e.PromoCodes).AutoInclude();

                entity.HasMany(p => p.Preferences)
                    .WithMany(p => p.Customers)
                    .UsingEntity<CustomerPreference>(
                        j => j
                            .HasOne(e => e.Preference)
                            .WithMany()
                            .HasForeignKey(pt => pt.PreferenceId),
                        j => j
                            .HasOne(pt => pt.Customer)
                            .WithMany()
                            .HasForeignKey(pt => pt.CustomerId),
                        j =>
                        {
                            j.HasKey(t => new { t.CustomerId, t.PreferenceId });
                        });

                entity.HasMany(e => e.PromoCodes)
                    .WithOne();
            });

            modelBuilder.Entity<Preference>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasValueGenerator<IdGenerator>();
            });

            modelBuilder.Entity<PromoCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasValueGenerator<IdGenerator>();
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.ServiceInfo).HasMaxLength(50);
                entity.Property(e => e.PartnerName).HasMaxLength(50);

                entity.Navigation(e => e.Preference).AutoInclude();
                entity.Navigation(e => e.PartnerManager).AutoInclude();

                entity.HasOne(p => p.PartnerManager)
                    .WithMany();
                entity.HasOne(p => p.Preference)
                    .WithMany();
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
