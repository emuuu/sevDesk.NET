using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class PartClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsParts()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Widget", partNumber = "W-001", price = 9.99m },
                new { id = 2, name = "Service", partNumber = "S-001", price = 150.00m }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Parts.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].Name.ShouldBe("Widget");
        result.Items[0].PartNumber.ShouldBe("W-001");
        result.Items[1].Price.ShouldBe(150.00m);
    }

    [Fact]
    public async Task GetAsync_ReturnsPart()
    {
        var responseBody = new
        {
            objects = new { id = 42, name = "Test Part", taxRate = 19.0m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Parts.GetAsync(42);

        result.Id.ShouldBe(42);
        result.Name.ShouldBe("Test Part");
        result.TaxRate.ShouldBe(19.0m);
    }
}
