using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Services;
using Otus.Teaching.PromoCodeFactory.WebHost.Mapper;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MapperProfile>();
            });

            var mapper = mappingConfig.CreateMapper();
            return services.AddSingleton(mapper);
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<ICustomerService, CustomerService>()
                           .AddScoped<IPreferenceService, PreferenceService>()
                           .AddScoped<IPromoCodeService, PromoCodeService>();
        }

    }
}
