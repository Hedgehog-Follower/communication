using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sending.API.HttpClients
{
    public class HttpClientForSingletonConsumers : IHttpClientForSingletonConsumers
    {
        private readonly IServiceProvider _serviceProvider;

        public HttpClientForSingletonConsumers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITestClient GetTestClient()
        {
            return _serviceProvider.GetRequiredService<ITestClient>();
        }
    }

    public interface IHttpClientForSingletonConsumers
    {
        ITestClient GetTestClient();
    }
}
