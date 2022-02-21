using System;
using AspNetCoreUserDataAuthorization.Authorization;
using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Authorization;
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

            services.AddDbContext<ApplicationContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("ContactContext");
                Console.WriteLine($"DB connectionString = {connectionString}");
                if (Env.IsDevelopment())
                {
                    Console.WriteLine($"{nameof(ConfigureServices)} called in Development environment.");
                    options.UseSqlite(connectionString);
                }
                else
                {
                    Console.WriteLine($"{nameof(ConfigureServices)} called in Production environment.");
                    options.UseNpgsql(connectionString);
                }
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                if (Env.IsDevelopment())
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                }
                else
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                }
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // Authorization handlers
            services.AddScoped<IAuthorizationHandler, ContactIsOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ContactManagerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ContactAdministratorAuthorizationHandler>();
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
