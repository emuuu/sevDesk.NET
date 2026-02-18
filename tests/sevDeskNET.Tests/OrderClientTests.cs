using System.Net;
using System.Net.Http.Json;
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
}
