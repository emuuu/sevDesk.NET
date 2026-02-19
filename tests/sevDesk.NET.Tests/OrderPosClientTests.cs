using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class OrderPosClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsOrderPositions()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Consulting", quantity = 10.00m, price = 120.00m }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.OrderPositions.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Name.ShouldBe("Consulting");
        result.Items[0].Quantity.ShouldBe(10.00m);
    }

    [Fact]
    public async Task GetAsync_ReturnsOrderPosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Development", quantity = 1.00m, price = 5000.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.OrderPositions.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Development");
        result.Price.ShouldBe(5000.00m);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedOrderPosition()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "New Order Pos", quantity = 2.00m, price = 50.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.OrderPositions.CreateAsync(new OrderPos
        {
            Name = "New Order Pos",
            Quantity = 2,
            Price = 50.00m
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("New Order Pos");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedOrderPosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Updated Order Pos", quantity = 3.00m, price = 75.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.OrderPositions.UpdateAsync(5, new OrderPos
        {
            Name = "Updated Order Pos"
        });

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Updated Order Pos");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.OrderPositions.DeleteAsync(1);
    }
}
