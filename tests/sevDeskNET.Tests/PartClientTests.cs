using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
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

    [Fact]
    public async Task CreateAsync_ReturnsCreatedPart()
    {
        var responseBody = new
        {
            objects = new { id = 50, name = "New Part", partNumber = "NP-001", price = 25.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Parts.CreateAsync(new Part
        {
            Name = "New Part",
            PartNumber = "NP-001",
            Price = 25.00m
        });

        result.Id.ShouldBe(50);
        result.Name.ShouldBe("New Part");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedPart()
    {
        var responseBody = new
        {
            objects = new { id = 42, name = "Updated Part", price = 30.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Parts.UpdateAsync(42, new Part
        {
            Name = "Updated Part",
            Price = 30.00m
        });

        result.Id.ShouldBe(42);
        result.Name.ShouldBe("Updated Part");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Parts.DeleteAsync(1);
    }
}
