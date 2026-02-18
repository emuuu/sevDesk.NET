using System.Text.Json;

namespace sevDeskNET.Internal;

internal static class SevDeskErrorHelper
{
    internal static string TryParseErrorDetail(string rawBody)
    {
        try
        {
            using var doc = JsonDocument.Parse(rawBody);
            if (doc.RootElement.TryGetProperty("error", out var error))
            {
                if (error.ValueKind == JsonValueKind.Object && error.TryGetProperty("message", out var msg))
                {
                    return msg.GetString() ?? rawBody;
                }

                if (error.ValueKind == JsonValueKind.String)
                {
                    return error.GetString() ?? rawBody;
                }
            }

            if (doc.RootElement.TryGetProperty("message", out var message))
            {
                return message.GetString() ?? rawBody;
            }
        }
        catch (JsonException)
        {
            // Fall through — raw body is not valid JSON
        }

        return rawBody;
    }
}
