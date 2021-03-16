using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sending.API.Options.HealthChecks;

namespace Sending.API.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddUriHealthCheck(
            this IHealthChecksBuilder builder, IConfigurationSection configurationSection)
        {
            var configuration = BindTo<UriHealthCheckConfiguration>(configurationSection);

            return 
                builder
                    .AddUrlGroup(
                        new Uri(configuration.BaseAddress),
                        configuration.Name,
                        configuration.HealthStatus,
                        configuration.Tags, 
                        TimeSpan.FromSeconds(configuration.Timeout));
        }

        public static IHealthChecksBuilder AddDatabaseContextCheck<TContext>(
            this IHealthChecksBuilder builder, IConfigurationSection configurationSection)
            where TContext : DbContext
        {
            var configuration = BindTo<BaseHealthCheckConfiguration>(configurationSection);

            return 
                builder
                    .AddDbContextCheck<TContext>(
                        configuration.Name,
                        configuration.HealthStatus,
                        configuration.Tags);
        }

        private static TConfiguration BindTo<TConfiguration>(IConfigurationSection configurationSection)
            where TConfiguration : BaseHealthCheckConfiguration, new()
        {
            if (configurationSection == null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            var configuration = new TConfiguration();
            configurationSection.Bind(configuration);

            return configuration;
        }
    }
}
