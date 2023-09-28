using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess;

public class LessonDatabaseContext: DbContext {
    public LessonDatabaseContext(DbContextOptions options)
        : base(options) { }

    public LessonDatabaseContext() { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<CustomerPreference> CustomerPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PromoCode>()
            .HasOne(promoCode => promoCode.Preference)
            .WithOne(preference => preference.PromoCode).HasForeignKey<PromoCode>(x => x.PreferenceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(employee => employee.Role)
            .WithOne(role => role.Employee).HasForeignKey<Employee>(x => x.RoleId);

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Preferences)
            .WithMany(preference => preference.Customers)
            .UsingEntity<CustomerPreference>(
                c => c.HasOne<Preference>().WithMany().HasForeignKey(x => x.PreferenceId)
                    .OnDelete(DeleteBehavior.Cascade),
                p => p.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade));

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.PromoCodes)
            .WithOne(code => code.Customer).HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}