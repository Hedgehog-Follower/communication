using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sending.API.HttpClients
{
    public static class HttpContentExtensions
    {
        public static async Task<TModel> ReadAndDeserializeFromJson<TModel>(this HttpContent content)
        {
            if (!(content is object))
            {
                throw new ArgumentException("Cannot deserialize to Json format from other type than object");
            }

            var stream = await content.ReadAsStreamAsync();

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Cannot read from stream");
            }

            try
            {
                return await JsonSerializer.DeserializeAsync<TModel>(stream);
            }
            catch (JsonException ex)
            {
                throw;
            }
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonHttpClient<TModel>(this HttpClient client, string uri, TModel model)
        {
            return await client.PostAsync(uri, new JsonContent<TModel>(model, new JsonSerializerOptions())).ConfigureAwait(false);
        }
    }

    public class JsonContent<TModel> : HttpContent
    {
        private readonly TModel _toSerialize;
        private readonly JsonSerializerOptions _options;

        public JsonContent(TModel toSerialize, JsonSerializerOptions options)
        {
            _toSerialize = toSerialize;
            _options = options;
            Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = Encoding.UTF8.WebName
            };
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return JsonSerializer.SerializeAsync(stream, _toSerialize, _options);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
