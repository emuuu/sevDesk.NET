using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class VoucherClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsVouchers()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, description = "Büromaterial", status = 100, voucherType = "VOU" },
                new { id = 2, description = "Software", status = 1000, voucherType = "VOU" }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Vouchers.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].Description.ShouldBe("Büromaterial");
        result.Items[1].Status.ShouldBe(Models.Enums.VoucherStatus.Paid);
    }

    [Fact]
    public async Task GetAsync_ReturnsVoucher()
    {
        var responseBody = new
        {
            objects = new { id = 10, description = "Test Voucher", status = 50, sumNet = "42.00" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Vouchers.GetAsync(10);

        result.Id.ShouldBe(10);
        result.Description.ShouldBe("Test Voucher");
        result.Status.ShouldBe(Models.Enums.VoucherStatus.Draft);
        result.SumNet.ShouldBe(42.00m);
    }
}
