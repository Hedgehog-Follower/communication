using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Sending.API.Options.Clients;
using Sending.API.Policies;
using Sending.Domain.Players;

namespace Sending.API.HttpClients
{
    public interface ITestClient
    {
        Task<string> GetAsync();
        Task<string> PostAsync();
    }

    public class TestClient : ITestClient
    {
        private readonly HttpClient _client;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly ILogger<TestClient> _logger;

        public TestClient(ITestConfiguration configuration, HttpClient client, IReadOnlyPolicyRegistry<string> policyRegistry, ILogger<TestClient> logger)
        {
            _client = client;
            _policyRegistry = policyRegistry;
            _logger = logger;
            _client.BaseAddress = new Uri(configuration.BaseAddress);
        }

        public async Task<string> GetAsync()
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.RetryWithLogging)
                              ?? Policy.NoOpAsync<HttpResponseMessage>();

            var context = new Context(
                $"{ContextNames.Logger}-{Guid.NewGuid()}",
                new Dictionary<string, object>
                    {
                        { ContextNames.Logger, _logger }
                    });

            var response = await retryPolicy.ExecuteAsync(ctx => _client.GetAsync("/ds"), context);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Error";
        }

        public async Task<string> PostAsync()
        {
            var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.RetryWithLogging)
                              ?? Policy.NoOpAsync<HttpResponseMessage>();

            var context = new Context(
                $"{ContextNames.Logger}-{Guid.NewGuid()}",
                new Dictionary<string, object>
                {
                    { ContextNames.Logger, _logger }
                });


            var request = new HttpRequestMessage(HttpMethod.Post, "/players");
            request.Content = new JsonContent<Player>(new Player { FirstName = "Olek", LastName = "Malinowski" }, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            });

            _logger.LogWarning("URL Receiving: {0}", _client.BaseAddress.ToString());
            var response = await _client.PostJsonHttpClient("/players", new Player {FirstName = "Olek", LastName = "Malinowski"});
            //var respose = await _client.PostAsync("/players", new JsonContent(
            //    new Player {FirstName = "Olek", LastName = "Malinowski"}, new JsonSerializerOptions
            //    {
            //        WriteIndented = true,
            //        PropertyNameCaseInsensitive = true
            //    }));

            //var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            //var response = await retryPolicy.ExecuteAsync(ctx => _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead), context);

            //return string.Empty;
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Error";
        }
    }
}
