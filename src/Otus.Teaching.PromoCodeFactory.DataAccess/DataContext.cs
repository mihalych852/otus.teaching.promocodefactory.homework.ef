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





        public DataContext(DbContextOptions<DbContext> dbContextOptions): base(dbContextOptions)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                .HasOne<Role>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.Role);



            modelBuilder.Entity<Preference>()
                .HasMany<Customer>()
                .WithMany(c => c.Preferences);
                //.Map(cp =>
                //    {
                //    cp.MapLeftKey("CustomerRefId");
                //    cp.MapRightKey("PreferenceRefId");
                //    cp.ToTable("CustomerPreference");
                //    }
                //);



        }

    }
}
