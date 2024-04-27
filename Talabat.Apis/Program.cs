using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Apis.Extensions;
using Talabat.Apis.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Data.Contexts;
using Talabat.Repository.Identity;

namespace Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var bulider = WebApplication.CreateBuilder(args);

            #region Run Redis server
            Process.Start("redis-server"); 
            #endregion

            #region Configure Services
            bulider.Services.AddControllers();

            //Allow Dependancy injection for StoreDbContext
            bulider.Services.AddDbContext<StoreContext>(options
                => options.UseSqlServer(bulider.Configuration.GetConnectionString("DefaultConnection")));

            //Allow Dependancy injection for RedisDbContext
            bulider.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = bulider.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection);
            });

            //Allow Dependancy injection for IdentityDbContext
            bulider.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(bulider.Configuration.GetConnectionString("IdentityConnection")));


            //ApplicationServicesExtensions.ApplicationServices(services);
            bulider.Services.AddApplicationServices(); // call it as Extension method

            bulider.Services.AddSwaggerServices();

            bulider.Services.AddIdentityServices(bulider.Configuration);

            bulider.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });
            #endregion

            var app = bulider.Build();

            #region Apply Migrations And Data Seeding
            var services = app.Services;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<StoreContext>();
                await context.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(context, loggerFactory);

                var IdentityContext = services.GetRequiredService<AppIdentityDbContext>();
                await IdentityContext.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, ex.Message);
            }
            #endregion

            #region Configure HTTP Request Pipelines

            app.UseMiddleware<ExceptionMiddleware>();
                if (app.Environment.IsDevelopment())
                {
                    //app.UseDeveloperExceptionPage();
                    app.UseSwaggerDocumentation();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors("CorsPolicy");
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            #endregion

            app.Run();  
        }


    }
}


