using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sending.API.HttpClients
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder RegisterAndAddHttpMessageHandler<THandler>(this IHttpClientBuilder builder)
            where THandler : DelegatingHandler
        {
            if(builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddTransient<THandler>();

            builder.AddHttpMessageHandler<THandler>();

            return builder;
        }
    }
}
