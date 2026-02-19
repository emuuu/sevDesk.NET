using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class OrderClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    private static SevDeskClient CreateSequentialClient(params HttpResponseMessage[] responses) =>
        new(new HttpClient(new SequentialMockHttpMessageHandler(responses))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsOrders()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, orderNumber = "AN-001", status = 100, orderType = "AN" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Orders.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].OrderNumber.ShouldBe("AN-001");
        result.Items[0].Status.ShouldBe(Models.Enums.OrderStatus.Draft);
        result.Items[0].OrderType.ShouldBe(Models.Enums.OrderType.AN);
    }

    [Fact]
    public async Task GetAsync_ReturnsOrder()
    {
        var responseBody = new
        {
            objects = new { id = 5, orderNumber = "AB-005", status = 500, sumGross = "1190.00" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Orders.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Status.ShouldBe(Models.Enums.OrderStatus.Accepted);
        result.SumGross.ShouldBe(1190.00m);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedOrder()
    {
        var responseBody = new
        {
            objects = new { id = 10, orderNumber = "AN-010", status = 100, orderType = "AN" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Orders.CreateAsync(new Order
        {
            OrderNumber = "AN-010",
            Currency = "EUR"
        });

        result.Id.ShouldBe(10);
        result.OrderNumber.ShouldBe("AN-010");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedOrder()
    {
        var responseBody = new
        {
            objects = new { id = 5, orderNumber = "AB-005-Updated", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Orders.UpdateAsync(5, new Order
        {
            OrderNumber = "AB-005-Updated"
        });

        result.Id.ShouldBe(5);
        result.OrderNumber.ShouldBe("AB-005-Updated");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Orders.DeleteAsync(1);
    }

    [Fact]
    public async Task SaveOrderAsync_ReturnsOrder()
    {
        var factoryResponse = new StringContent(
            """{"objects":{"order":{"id":15}}}""",
            System.Text.Encoding.UTF8, "application/json");

        var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new
            {
                objects = new { id = 15, orderNumber = "AN-015", status = 100 }
            })
        };

        var client = CreateSequentialClient(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = factoryResponse },
            getResponse);

        var result = await client.Orders.SaveOrderAsync(
            new Order { OrderNumber = "AN-015" },
            [new OrderPos { Name = "Position 1", Quantity = 1, Price = 200 }]);

        result.Id.ShouldBe(15);
        result.OrderNumber.ShouldBe("AN-015");
    }

    [Fact]
    public async Task ChangeStatusAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Orders.ChangeStatusAsync(1, Models.Enums.OrderStatus.Accepted);
    }

    [Fact]
    public async Task GetPdfAsync_ReturnsPdfBytes()
    {
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(pdfBytes)
        });

        var result = await client.Orders.GetPdfAsync(1);

        result.ShouldBe(pdfBytes);
    }

    [Fact]
    public async Task SendViaEmailAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Orders.SendViaEmailAsync(1, "test@example.com", "Order", "Please review");
    }

    [Fact]
    public async Task DuplicateAsync_ReturnsDuplicatedOrder()
    {
        var responseBody = new
        {
            objects = new { id = 11, orderNumber = "AN-011", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Orders.DuplicateAsync(10);

        result.Id.ShouldBe(11);
    }
}
