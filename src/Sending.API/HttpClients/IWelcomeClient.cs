using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sending.API.Options.Clients;

namespace Sending.API.HttpClients
{
    public interface IWelcomeClient
    {
        Task<string> GetAsync();
    }

    public class WelcomeClient : IWelcomeClient
    {
        private readonly HttpClient _client;

        public WelcomeClient(ITestConfiguration configuration, HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }

        public async Task<string> GetAsync()
        {
            // You may have noticed that HttpResponseMessage implements IDisposable since it’s possible that it can hold onto OS resources.
            // This is true only in the scenario where we choose the ResponseHeadersRead option.
            // When using this option, we accept more responsibility around system resources, since the connection to the remote server is tied up until we decide
            //   that we’re done with the content.The way we signal that is by disposing of the HttpResponseMessage, which then frees up the connection
            //   to be used for other requests.
            using var response = await _client.GetAsync("/re", HttpCompletionOption.ResponseHeadersRead);
            var configuration = response.Content.ReadAndDeserializeFromJson<TestConfiguration>();

            return string.Empty;
        }
    }
}
