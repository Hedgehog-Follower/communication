using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sending.Domain;

namespace Sending.API.Hosts
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            if (environment.IsDevelopment())
            {
                using var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                try
                {
                    logger.LogWarning("Before DB migration");
                    applicationContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error on DB Migration");
                    // Log here when migration fail
                }
            }

            return host;
        }
    }
}
