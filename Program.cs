using System;
using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreUserDataAuthorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationContext>();
                    Console.WriteLine($"{nameof(context)} has been found: {context}");
                    context.Database.Migrate();

                    var config = host.Services.GetRequiredService<IConfiguration>();
                    var testUserPw = config["SeedUserPW"];

                    SeedData.Initialize(services, testUserPw).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred seeding the DB: {e}");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
