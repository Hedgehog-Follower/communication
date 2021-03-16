using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sending.API.Options.HealthChecks
{
    public class BaseHealthCheckConfiguration
    {
        public int Timeout { get; set; }
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public int Status { get; set; }
        public HealthStatus HealthStatus => (HealthStatus) Status;
    }
}
