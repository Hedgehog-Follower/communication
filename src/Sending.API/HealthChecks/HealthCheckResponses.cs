using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sending.API.HealthChecks
{
    public static class HealthCheckResponses
    {
        public static Task WriteJsonResponseForReady(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var options = new JsonWriterOptions { Indented = true };
            using var writer = new Utf8JsonWriter(context.Response.BodyWriter, options);

            writer.WriteStartObject();
            writer.WriteString("status", report.Status.ToString());
            writer.WriteString("totalDuration", report.TotalDuration.ToString());

            if (report.Entries.Count > 0)
            {
                writer.WriteStartArray("results");
                foreach (var (key, value) in report.Entries)
                {
                    writer.WriteStartObject();
                    writer.WriteString("key", key);
                    writer.WriteString("status", value.Status.ToString());
                    writer.WriteString("description", value.Description);
                    writer.WriteString("duration", value.Duration.ToString());
                    writer.WriteStartArray("tags");
                    foreach (var valueTag in value.Tags)
                    {
                        writer.WriteStringValue(valueTag);
                    }
                    writer.WriteEndArray();
                    writer.WriteStartArray("data");
                    foreach (var (dataKey, dataValue) in value.Data.Where(d => d.Value is object))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(dataKey);
                        JsonSerializer.Serialize(writer, dataValue, dataValue.GetType());
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                    // In some point we could collect exception details.
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();

            return Task.CompletedTask;
        }

        public static Task WriteJsonResponseForLive(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var options = new JsonWriterOptions { Indented = true };
            using var writer = new Utf8JsonWriter(context.Response.BodyWriter, options);

            writer.WriteStartObject();
            writer.WriteString("status", report.Status.ToString());
            writer.WriteString("totalDuration", report.TotalDuration.ToString());
            writer.WriteEndObject();

            return Task.CompletedTask;
        }
    }
}