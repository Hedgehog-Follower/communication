using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sending.API.HealthChecks
{
    public static class HealthCheckRouteBuilderExtensions
    {
        private static readonly IDictionary<HealthStatus, int> HealthStatuses = new Dictionary<HealthStatus, int>
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        };
        public static IEndpointRouteBuilder MapDefaultHealthChecks(this IEndpointRouteBuilder builder)
        {
            builder.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponses.WriteJsonResponseForLive,
                AllowCachingResponses = false,
                ResultStatusCodes = HealthStatuses,
                Predicate = check => check.Tags.Contains("live")
            });

            builder.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponses.WriteJsonResponseForReady,
                AllowCachingResponses = false,
                ResultStatusCodes = HealthStatuses,
                Predicate = check => check.Tags.Contains("ready")
            });

            return builder;
        }
    }
}
