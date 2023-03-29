using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using tus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await CreateDbIfNotExists(host);
            await host.RunAsync();
        }

        /// <summary>
        /// Создание хоста
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// Переинициализация базы данных
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static async Task CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            await DbInitializer.InitializeAsync(context);
        }
    }
}