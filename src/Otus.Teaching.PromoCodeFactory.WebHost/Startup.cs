using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.WebHost.Mapping;
using tus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; set;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Default")));
            services.AddControllers();
            services.AddScoped(typeof(IRepository<Customer>), typeof(EfRepository<Customer>));
            services.AddScoped(typeof(IRepository<Employee>), typeof(EfRepository<Employee>));
            services.AddScoped(typeof(IRepository<Preference>), typeof(EfRepository<Preference>));
            services.AddScoped(typeof(IRepository<PromoCode>), typeof(EfRepository<PromoCode>));
            services.AddScoped(typeof(IRepository<Role>), typeof(EfRepository<Role>));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}