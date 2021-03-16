using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace Sending.API.Policies
{
    public static class PollyPolicies
    {
        public static IServiceCollection AddPollyPolicies(this IServiceCollection services)
        {
            services
                .AddPolicyRegistry()
                .AddRetryPolicy()
                .AddTimeoutPolicy()
                .AddRetryWithLoggingPolicy();

            return services;
        }

        public static IPolicyRegistry<string> AddRetryPolicy(this IPolicyRegistry<string> policyRegistry)
        {
            var retryPolicy = 
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .WithPolicyKey(PolicyNames.Retry);

            policyRegistry.Add(PolicyNames.Retry, retryPolicy);
            return policyRegistry;
        }

        public static IPolicyRegistry<string> AddTimeoutPolicy(this IPolicyRegistry<string> registerPolicy)
        {
            var timeoutPolicy =
                Policy
                    .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(15))
                    .WithPolicyKey(PolicyNames.Timeout);

            registerPolicy.Add(PolicyNames.Timeout, timeoutPolicy);
            return registerPolicy;
        }

        public static IPolicyRegistry<string> AddRetryWithLoggingPolicy(this IPolicyRegistry<string> policyRegistry)
        {
            var retryPolicy =
                Policy
                    .Handle<Exception>()
                    .OrResult<HttpResponseMessage>(msg => !msg.IsSuccessStatusCode)
                    .WaitAndRetryAsync(2, retryCount => TimeSpan.FromSeconds(3),
                        (result, timeSpan, retryCount, context) =>
                        {
                            if (!context.TryGetLogger(out var logger)) return;

                            if (result.Exception != null)
                            {
                                logger.LogError(result.Exception, "An exception occurred on retry {RetryAttempt} for {PolicyKey}", retryCount, context.PolicyKey);
                            }
                            else
                            {
                                logger.LogError("An non success code {StatusCode} was received on retry {RetryAttempt} for {PolicyKey}", 
                                    (int)result.Result.StatusCode, retryCount, context.PolicyKey);
                            }
                        })
                    .WithPolicyKey(PolicyNames.RetryWithLogging);

            policyRegistry.Add(PolicyNames.RetryWithLogging, retryPolicy);
            return policyRegistry;
        }
    }
}
