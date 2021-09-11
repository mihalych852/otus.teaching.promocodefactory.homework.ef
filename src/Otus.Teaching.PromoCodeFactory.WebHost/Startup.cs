using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories.EntityFrameworkRepositories;
using Otus.Teaching.PromoCodeFactory.WebHost.Infrastructure.Exceptions;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IRepository<Employee>, EntityFrameworkRepository<Employee>>();
            services.AddScoped<IRepository<Role>, EntityFrameworkRepository<Role>>();
            services.AddScoped<IRepository<Preference>, PreferencesEntityFrameworkRepository>();
            services.AddScoped<IRepository<Customer>, CustomersEntityFrameworkRepository>();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });

            services.AddDbContext<PromoCodeDataContext>(builder =>
            {
                builder.UseSqlite(_configuration.GetConnectionString("SQLite"));
            });

            services.AddAutoMapper(typeof(Startup));
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
                app.UseJsonExceptionHandler();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x => { x.DocExpansion = "list"; });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}