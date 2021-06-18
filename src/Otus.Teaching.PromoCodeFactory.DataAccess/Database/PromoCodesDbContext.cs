using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Database
{
    public class PromoCodesDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Preference> Preferences { get; set; }
        
        public DbSet<PromoCode> PromoCodes { get; set; }

        public PromoCodesDbContext(DbContextOptions<PromoCodesDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Role>()
                .ToTable("Role")
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Role>()
                .Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(512);
            
            modelBuilder.Entity<Customer>()
                .ToTable("Customer")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Preferences)
                .WithMany(p => p.Customers)
                .UsingEntity(j => j.ToTable("CustomerPreference"));

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.PromoCodes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<Preference>()
                .ToTable("Preference")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Customers)
                .WithMany(c => c.Preferences)
                .UsingEntity(j => j.ToTable("CustomerPreference"));

            modelBuilder.Entity<Preference>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<PromoCode>()
                .ToTable("PromoCode")
                .HasKey(p => p.Id);

            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.Preference)
                .WithMany();

            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.PartnerManager)
                .WithMany();

            modelBuilder.Entity<PromoCode>()
                .Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<PromoCode>()
                .Property(p => p.PartnerName)
                .IsRequired()
                .HasMaxLength(256);
            
            modelBuilder.Entity<PromoCode>()
                .Property(p => p.ServiceInfo)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<PromoCode>()
                .Property(p => p.BeginDate)
                .IsRequired();
            
            modelBuilder.Entity<PromoCode>()
                .Property(p => p.EndDate)
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetEntryIdentifierIfEmpty();
            
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            SetEntryIdentifierIfEmpty();
            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            SetEntryIdentifierIfEmpty();
            
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetEntryIdentifierIfEmpty();
            
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void SetEntryIdentifierIfEmpty()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => 
                e.State == EntityState.Added
                && e.Metadata.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                var baseEntity = (BaseEntity)entry.Entity;

                if(baseEntity.Id == Guid.Empty)
                    baseEntity.Id = Guid.NewGuid();
            }
        }
    }
}