using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

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
                new { id = 1, name = "Girokonto", iban = "DE89370400440532013000", currency = "EUR", type = "online" }
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
            objects = new { id = 5, name = "Sparkasse", type = "offline", currency = "EUR" }
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

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCheckAccount()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "Neues Konto", type = "online", currency = "EUR", iban = "DE123" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccounts.CreateAsync(new CheckAccount
        {
            Name = "Neues Konto",
            Currency = "EUR"
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("Neues Konto");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedCheckAccount()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Sparkasse Updated", type = "offline", currency = "EUR" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccounts.UpdateAsync(5, new CheckAccount
        {
            Name = "Sparkasse Updated"
        });

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Sparkasse Updated");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CheckAccounts.DeleteAsync(1);
    }

    [Fact]
    public async Task GetBalanceAsync_ReturnsBalance()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"objects":1234.56}""", System.Text.Encoding.UTF8, "application/json")
        });

        var result = await client.CheckAccounts.GetBalanceAsync(5);

        result.ShouldBe(1234.56m);
    }
}
