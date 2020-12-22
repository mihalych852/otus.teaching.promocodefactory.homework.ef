using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DatabaseContextDesignTimeFactory: IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(new DirectoryInfo("../Otus.Teaching.PromoCodeFactory.WebHost").FullName)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
            
            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            System.Console.WriteLine(connectionString);
            
            builder.UseSqlite(connectionString);
            return new DataContext(builder.Options);
            
        }
    }
}