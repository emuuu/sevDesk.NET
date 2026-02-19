using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class InvoicePosClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsInvoicePositions()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Widget", quantity = 2.00m, price = 9.99m, positionNumber = 1 }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.InvoicePositions.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Name.ShouldBe("Widget");
        result.Items[0].Quantity.ShouldBe(2.00m);
    }

    [Fact]
    public async Task GetAsync_ReturnsInvoicePosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Service", quantity = 1.00m, price = 150.00m, taxRate = 19m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.InvoicePositions.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Service");
        result.Price.ShouldBe(150.00m);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedInvoicePosition()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "New Position", quantity = 3.00m, price = 25.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.InvoicePositions.CreateAsync(new InvoicePos
        {
            Name = "New Position",
            Quantity = 3,
            Price = 25.00m
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("New Position");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedInvoicePosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Updated Position", quantity = 5.00m, price = 200.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.InvoicePositions.UpdateAsync(5, new InvoicePos
        {
            Name = "Updated Position",
            Quantity = 5
        });

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Updated Position");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.InvoicePositions.DeleteAsync(1);
    }
}
