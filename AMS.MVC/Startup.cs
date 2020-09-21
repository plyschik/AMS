using AMS.MVC.Authorization;
using AMS.MVC.Data;
using AMS.MVC.Data.Models;
using AMS.MVC.Repositories;
using AMS.MVC.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            services.AddHttpContextAccessor();
            
            services.AddFlashMessage();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IStarService, StarService>();

            services.AddScoped<IAuthorizationHandler, MovieEditAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MovieStarCreateAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MovieStarEditAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MovieStarDeleteAuthorizationHandler>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseHsts();
            }

            application.UseStatusCodePagesWithReExecute("/Error/{0}");

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
