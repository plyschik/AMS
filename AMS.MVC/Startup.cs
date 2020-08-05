using AMS.MVC.Authorization;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vereyon.Web;

namespace AMS.MVC
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(builder =>
            {
                builder.UseNpgsql(_configuration.GetConnectionString("Default"));
            });

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddFlashMessage();
            
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            
            services.AddScoped<IAuthorizationHandler, MovieEditAuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler("/Home/Error");
                application.UseHsts();
            }

            application.UseHttpsRedirection();
            application.UseStaticFiles();
            
            application.UseRouting();
            
            application.UseAuthentication();
            application.UseAuthorization();

            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Movies}/{action=Index}/{id?}"
                );

                endpoints.MapRazorPages();
            });
        }
    }
}
