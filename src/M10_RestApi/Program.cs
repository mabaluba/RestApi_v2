using System;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace M10_RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("init Main");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    try
                    {
                        var services = scope.ServiceProvider;
                        var context = services.GetRequiredService<EducationDbContext>();
                        var dbInitializer = services.GetRequiredService<IDbInitializer>();
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        dbInitializer.Initialize(context);
                    }
                    catch (Exception exception)
                    {
                        logger.Error(exception, "An error occurred while seeding the database.");
                    }
                }

                host.Run();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of Exception in Main.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                // .ConfigureLogging(logging =>
                // {
                //     logging.ClearProviders();
                //     logging.AddConsole();
                //     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                // })
                .UseNLog();
    }
}