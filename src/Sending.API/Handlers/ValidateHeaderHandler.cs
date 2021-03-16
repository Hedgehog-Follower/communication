using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sending.API.Handlers
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("X-API-KEY"))
                return new HttpResponseMessage
                {
                    Content = new StringContent("X-API-KEY header must be provided"),
                    StatusCode = HttpStatusCode.BadRequest
                };

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
