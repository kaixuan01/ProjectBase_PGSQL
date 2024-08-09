using DAL;
using DAL.Data;
using EFCore.AutomaticMigrations;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Extension
{
    public static class StartUpExtension
    {
        public static async void CreatOrUpdateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var myContext = services.GetRequiredService<MyDbContext>();
            // Create database if there is no exist
            myContext.Database.EnsureCreated();

            // Upgrate database to latest version
            myContext.MigrateToLatestVersion();

            // Auto insert default data (Keep it if needed)
            DBInitializerSeedData.InitializeDatabase(myContext);
        }
    }
}
