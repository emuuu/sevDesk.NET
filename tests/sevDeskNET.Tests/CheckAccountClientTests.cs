using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class CheckAccountClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsCheckAccounts()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Girokonto", iban = "DE89370400440532013000", currency = "EUR", type = 0 }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccounts.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Name.ShouldBe("Girokonto");
        result.Items[0].Iban.ShouldBe("DE89370400440532013000");
        result.Items[0].Type.ShouldBe(Models.Enums.CheckAccountType.Online);
    }

    [Fact]
    public async Task GetAsync_ReturnsCheckAccount()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Sparkasse", type = 1, currency = "EUR" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccounts.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Sparkasse");
        result.Type.ShouldBe(Models.Enums.CheckAccountType.Offline);
    }
}
