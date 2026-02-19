using sevDesk.NET.Docs;
using sevDesk.NET.Docs.Services;
using sevDesk.NET;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Mutable API token service (Singleton for WASM)
builder.Services.AddSingleton<DocsApiTokenService>();

// ISevDeskClient with mutable auth handler (bypasses AddSevDesk validation)
builder.Services.AddScoped<ISevDeskClient>(sp =>
{
    var tokenService = sp.GetRequiredService<DocsApiTokenService>();
    var handler = new DocsAuthHandler(tokenService) { InnerHandler = new HttpClientHandler() };
    var http = new HttpClient(handler)
    {
        BaseAddress = new Uri("https://my.sevdesk.de/api/v1/"),
        Timeout = TimeSpan.FromSeconds(30)
    };
    return new SevDeskClient(http);
});

// Doc services
builder.Services.AddScoped<IRestApiDocService, RestApiDocService>();
builder.Services.AddScoped<IDocContentService, DocContentService>();

await builder.Build().RunAsync();
