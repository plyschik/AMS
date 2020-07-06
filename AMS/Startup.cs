using System;
using AMS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AMS
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
                builder.UseMySql(_configuration.GetConnectionString("Default"), options =>
                {
                    options.ServerVersion(new Version(8, 0, 20), ServerType.MySql);
                });
            });
            
            services.AddControllers();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            
            application.UseHttpsRedirection();

            application.UseSwagger();

            application.UseSwaggerUI(configuration =>
            {
                configuration.RoutePrefix = string.Empty;
                configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "AMS v1");
            });
            
            application.UseRouting();

            application.UseAuthorization();

            application.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
