using Microsoft.Extensions.DependencyInjection;

namespace sevDeskNET;

/// <summary>
/// Extension methods for registering sevDesk services with the dependency injection container.
/// </summary>
public static class SevDeskServiceCollectionExtensions
{
    /// <summary>
    /// Adds sevDesk client services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure <see cref="SevDeskOptions"/>.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSevDesk(this IServiceCollection services, Action<SevDeskOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptionsWithValidateOnStart<SevDeskOptions>()
            .Configure(configure)
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiToken), "sevDesk ApiToken is required.")
            .Validate(o => o.CustomBaseUrl is null || IsHttpsOrLocalhost(o.CustomBaseUrl),
                "CustomBaseUrl must use HTTPS.");

        services.AddTransient<SevDeskAuthHandler>();

        services.AddHttpClient<ISevDeskClient, SevDeskClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<SevDeskOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
        })
        .AddHttpMessageHandler<SevDeskAuthHandler>()
        .RedactLoggedHeaders(["Authorization"]);

        return services;
    }

    private static bool IsHttpsOrLocalhost(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;
        if (uri.Scheme == Uri.UriSchemeHttps) return true;
        return uri.Host is "localhost" or "127.0.0.1" or "::1";
    }
}
