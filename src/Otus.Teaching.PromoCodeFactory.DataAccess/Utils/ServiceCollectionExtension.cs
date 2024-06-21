using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Utils
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabaseContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:Main");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
            services.AddScoped<IMigrator, RecreateMigrator>();
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                    .AddScoped<IRepository<Employee>, EfRepository<Employee, ApplicationDbContext>>()
                    .AddScoped<IRepository<Role>, EfRepository<Role, ApplicationDbContext>>()
                    .AddScoped<IRepository<Customer>, EfRepository<Customer, ApplicationDbContext>>()
                    .AddScoped<IRepository<Preference>, EfRepository<Preference, ApplicationDbContext>>()
                    .AddScoped<IRepository<PromoCode>, EfRepository<PromoCode, ApplicationDbContext>>();
        }
    }
}
