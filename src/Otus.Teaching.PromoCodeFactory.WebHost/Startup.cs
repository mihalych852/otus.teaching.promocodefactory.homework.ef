using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //var applicationSettings = Configuration.Get<ApplicationSettings>();

            services.AddControllers();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            //services.AddScoped(typeof(IRepository<Customer>), typeof(EfRepository<Customer>));

            services//.AddSingleton(applicationSettings)
                    .AddSingleton((IConfigurationRoot)Configuration)
                    .AddDbContext<DatabaseContext>(optionsBuilder 
                        => optionsBuilder.UseSqlite(Configuration.GetValue<string>("ConnectionStrings:ConnectionStringLite"))
                            .EnableSensitiveDataLogging()
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); //applicationSettings.ConnectionStringLite));

            services.AddScoped<DbContext, DatabaseContext>();
            services.AddScoped(typeof(IFillDbWithInitialData), typeof(FillDbWithInitialData));

            //services.AddSingleton<Customer>();

            //services.AddScoped(typeof(IRepository<Employee>), (x) => 
            //    new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            //services.AddScoped(typeof(IRepository<Role>), (x) =>
            //    new InMemoryRepository<Role>(FakeDataFactory.Roles));
            //services.AddScoped(typeof(IRepository<Preference>), (x) =>
            //    new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
            //services.AddScoped(typeof(IRepository<Customer>), (x) =>
            //    new InMemoryRepository<Customer>(FakeDataFactory.Customers));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFillDbWithInitialData fillDb)
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
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //fillDb.Fill();
        }
    }
}