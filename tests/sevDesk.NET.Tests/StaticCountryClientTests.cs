using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class StaticCountryClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    private static (SevDeskClient Client, MockHttpMessageHandler Handler) CreateClientWithHandler(HttpResponseMessage response)
    {
        var handler = new MockHttpMessageHandler(response);
        return (new SevDeskClient(new HttpClient(handler) { BaseAddress = new Uri("https://my.sevdesk.de/api/v1/") }), handler);
    }

    [Fact]
    public async Task ListAsync_ReturnsCountries()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, code = "de", name = "Deutschland", nameEn = "Germany", translationCode = "COUNTRY_DE" },
                new { id = 2, code = "at", name = "Österreich", nameEn = "Austria", translationCode = "COUNTRY_AT" }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.StaticCountries.ListAsync();

        result.Total.ShouldBe(2);
        result.Items.Count.ShouldBe(2);
        result.Items[0].Code.ShouldBe("de");
        result.Items[0].Name.ShouldBe("Deutschland");
        result.Items[0].NameEn.ShouldBe("Germany");
        result.Items[1].Code.ShouldBe("at");
    }

    [Fact]
    public async Task ListAsync_RespectsPagination()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.StaticCountries.ListAsync(new PaginationParameters { Limit = 250, Offset = 100 });

        var query = handler.LastRequest!.RequestUri!.Query;
        query.ShouldContain("limit=250");
        query.ShouldContain("offset=100");
    }

    [Fact]
    public async Task GetAsync_ReturnsCountry()
    {
        var responseBody = new
        {
            objects = new { id = 1, code = "de", name = "Deutschland", nameEn = "Germany", locale = "de_DE", priority = 100 }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.StaticCountries.GetAsync(1);

        handler.LastRequest!.RequestUri!.AbsolutePath.ShouldEndWith("/StaticCountry/1");
        result.Id.ShouldBe(1);
        result.Code.ShouldBe("de");
        result.Locale.ShouldBe("de_DE");
        result.Priority.ShouldBe(100);
    }
}
