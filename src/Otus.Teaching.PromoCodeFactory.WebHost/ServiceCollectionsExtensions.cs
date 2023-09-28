using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

namespace Otus.Teaching.PromoCodeFactory.WebHost; 

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection ConfigureSqlServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LessonDatabaseContext>(
            options =>
            {
                var migrationsAssemblyName = typeof(LessonDatabaseContext).GetTypeInfo().Assembly.GetName().Name;
                var connectionString = configuration.GetConnectionString("SqlLite");
                options.UseSqlite(connectionString, x => x.MigrationsAssembly(migrationsAssemblyName));
             
            });
        
        services.AddScoped<IRepository<Employee>, EfRepository<Employee, LessonDatabaseContext>>();
        services.AddScoped<IRepository<Role>, EfRepository<Role, LessonDatabaseContext>>();
        services.AddScoped<IRepository<Customer>, EfRepository<Customer, LessonDatabaseContext>>();
        services.AddScoped<IRepository<CustomerPreference>, EfRepository<CustomerPreference, LessonDatabaseContext>>();
        services.AddScoped<IRepository<Preference>, EfRepository<Preference, LessonDatabaseContext>>();
        services.AddScoped<IRepository<PromoCode>, EfRepository<PromoCode, LessonDatabaseContext>>();
        
        return services;
    } 
    
    public static IServiceCollection ConfigureInMemoryServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<Employee>), (x) => 
            new InMemoryRepository<Employee>(FakeDataFactory.Employees));
        services.AddScoped(typeof(IRepository<Role>), (x) => 
            new InMemoryRepository<Role>(FakeDataFactory.Roles));
        services.AddScoped(typeof(IRepository<Preference>), (x) => 
            new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
        services.AddScoped(typeof(IRepository<Customer>), (x) => 
            new InMemoryRepository<Customer>(FakeDataFactory.Customers));
        
        return services;
    }
    
    public static void Migration(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<LessonDatabaseContext>();
        if (databaseContext.Database.IsRelational())
            databaseContext.Database.Migrate();
    }

    public static void AddTestData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        FakeDataFactory.Execute(scope.ServiceProvider);
    }
}