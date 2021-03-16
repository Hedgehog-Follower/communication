namespace Sending.API.Options.Clients
{
    public interface ITestConfiguration
    {
        string BaseAddress { get; set; }
    }

    public class TestConfiguration : ClientsConfiguration, ITestConfiguration
    {
        public static string ConfigurationName => $"{Section}Test";
        public string BaseAddress { get; set; }
    }
}
