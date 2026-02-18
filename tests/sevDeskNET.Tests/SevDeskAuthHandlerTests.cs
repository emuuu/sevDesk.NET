using Microsoft.Extensions.Options;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class SevDeskAuthHandlerTests
{
    [Fact]
    public async Task SendAsync_AddsAuthorizationHeader()
    {
        var options = Options.Create(new SevDeskOptions { ApiToken = "test-token-12345" });
        var handler = new SevDeskAuthHandler(options)
        {
            InnerHandler = new MockHttpMessageHandler(new HttpResponseMessage(System.Net.HttpStatusCode.OK))
        };

        var client = new HttpClient(handler) { BaseAddress = new Uri("https://my.sevdesk.de/api/v1/") };
        await client.GetAsync("Contact");

        var mockHandler = (MockHttpMessageHandler)handler.InnerHandler;
        mockHandler.LastRequest.ShouldNotBeNull();
        mockHandler.LastRequest.Headers.TryGetValues("Authorization", out var values).ShouldBeTrue();
        values!.First().ShouldBe("test-token-12345");
    }
}
