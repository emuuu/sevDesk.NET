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
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiToken), "sevDesk ApiToken is required.");

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
}
