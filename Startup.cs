using System;
using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreUserDataAuthorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<AspNetCoreUserDataAuthorizationContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("ContactContext");
                if (Env.IsDevelopment())
                {
                    Console.WriteLine($"{nameof(ConfigureServices)} called in Development environment, with connectionString={connectionString}");
                    options.UseSqlite(connectionString);
                }
                else
                {
                    Console.WriteLine($"{nameof(ConfigureServices)} called in Production environment, with connectionString={connectionString}");
                    options.UseNpgsql(connectionString);
                }
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AspNetCoreUserDataAuthorizationContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("Using Development environment.");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Console.WriteLine("Using Prod environment.");
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
