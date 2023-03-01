using System;
using System.Linq;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace M10_RestApi.Tests
{
    internal class CustomWebApplicationFactory<TStartup> :
        WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "5000");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<EducationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<EducationDbContext>(
                    options => options.UseInMemoryDatabase("InMemoryDatabase")
                );

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<EducationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<DbInitializer>>();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    try
                    {
                        var dbInitializer = new DbInitializer(logger);
                        dbInitializer.Initialize(context);
                        logger.LogInformation("UseInMemoryDatabase");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the test database. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}