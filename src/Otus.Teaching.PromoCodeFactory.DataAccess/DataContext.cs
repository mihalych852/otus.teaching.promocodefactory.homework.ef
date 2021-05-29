using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.EntityTypeConfigurations;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public class DataContext : DbContext
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
            optionsBuilder
                .UseSqlite("FileName=promocodefactory.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new PreferenceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PromoCodeTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new CustomerPreferenceTypeConfiguration());
            
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
            
            /* Нужна таблица с партнерами
            modelBuilder.Entity<PromoCode>()
                .HasOne(x => x.PartnerManager)
                .WithMany()
                .HasForeignKey(x => x.PartnerManagerId);
            */

            modelBuilder.Entity<PromoCode>()
                .HasOne(x => x.Preference)
                .WithMany()
                .HasForeignKey(x => x.PreferenceId);

            modelBuilder.Entity<PromoCode>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.PromoCodes)
                .HasForeignKey(x => x.CustomerId);
            base.OnModelCreating(modelBuilder);
        }

    }
}