using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class CurrencyExchangeRateClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsCurrencyExchangeRates()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, currencyFrom = "USD", currencyTo = "EUR", rate = 0.92m }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CurrencyExchangeRates.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].CurrencyFrom.ShouldBe("USD");
        result.Items[0].CurrencyTo.ShouldBe("EUR");
    }

    [Fact]
    public async Task GetAsync_ReturnsCurrencyExchangeRate()
    {
        var responseBody = new
        {
            objects = new { id = 5, currencyFrom = "GBP", currencyTo = "EUR", rate = 1.17m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CurrencyExchangeRates.GetAsync(5);

        result.Id.ShouldBe(5);
        result.CurrencyFrom.ShouldBe("GBP");
        result.Rate.ShouldBe(1.17m);
    }
}
