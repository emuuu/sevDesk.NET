using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class VoucherPosClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsVoucherPositions()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, net = 100.00m, taxRate = 19m, comment = "Office supplies" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.VoucherPositions.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Net.ShouldBe(100.00m);
        result.Items[0].Comment.ShouldBe("Office supplies");
    }

    [Fact]
    public async Task GetAsync_ReturnsVoucherPosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, net = 250.00m, taxRate = 19m, comment = "Software license" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.VoucherPositions.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Net.ShouldBe(250.00m);
        result.Comment.ShouldBe("Software license");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedVoucherPosition()
    {
        var responseBody = new
        {
            objects = new { id = 10, net = 75.00m, taxRate = 19m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.VoucherPositions.CreateAsync(new VoucherPos
        {
            Net = 75.00m,
            TaxRate = 19
        });

        result.Id.ShouldBe(10);
        result.Net.ShouldBe(75.00m);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedVoucherPosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, net = 300.00m, taxRate = 19m, comment = "Updated" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.VoucherPositions.UpdateAsync(5, new VoucherPos
        {
            Net = 300.00m
        });

        result.Id.ShouldBe(5);
        result.Net.ShouldBe(300.00m);
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.VoucherPositions.DeleteAsync(1);
    }
}
