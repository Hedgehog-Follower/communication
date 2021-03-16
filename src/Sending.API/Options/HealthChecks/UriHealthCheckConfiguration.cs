namespace Sending.API.Options.HealthChecks
{
    public class UriHealthCheckConfiguration : BaseHealthCheckConfiguration
    {
        public const string SectionPointer = "HealthChecks:Ready:Uris:";
        public string BaseAddress { get; set; }
    }
}
