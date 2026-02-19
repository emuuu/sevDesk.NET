using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class VoucherClientTests
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

    [Fact]
    public async Task CreateAsync_ReturnsCreatedVoucher()
    {
        var responseBody = new
        {
            objects = new { id = 20, description = "New Voucher", status = 50 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Vouchers.CreateAsync(new Voucher
        {
            Description = "New Voucher",
            CreditDebit = "D"
        });

        result.Id.ShouldBe(20);
        result.Description.ShouldBe("New Voucher");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedVoucher()
    {
        var responseBody = new
        {
            objects = new { id = 10, description = "Updated Voucher", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Vouchers.UpdateAsync(10, new Voucher
        {
            Description = "Updated Voucher"
        });

        result.Id.ShouldBe(10);
        result.Description.ShouldBe("Updated Voucher");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Vouchers.DeleteAsync(1);
    }

    [Fact]
    public async Task SaveVoucherAsync_ReturnsVoucher()
    {
        var factoryResponse = new StringContent(
            """{"objects":{"voucher":{"id":25}}}""",
            System.Text.Encoding.UTF8, "application/json");

        var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new
            {
                objects = new { id = 25, description = "Saved Voucher", status = 50 }
            })
        };

        var client = CreateSequentialClient(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = factoryResponse },
            getResponse);

        var result = await client.Vouchers.SaveVoucherAsync(
            new Voucher { Description = "Saved Voucher", CreditDebit = "D" },
            [new VoucherPos { Net = 100, TaxRate = 19 }]);

        result.Id.ShouldBe(25);
        result.Description.ShouldBe("Saved Voucher");
    }

    [Fact]
    public async Task BookAmountAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Vouchers.BookAmountAsync(1, 42.00m, 5, DateTime.Today);
    }

    [Fact]
    public async Task MarkAsPaidAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Vouchers.MarkAsPaidAsync(1);
    }

    [Fact]
    public async Task MarkAsOpenAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Vouchers.MarkAsOpenAsync(1);
    }

    [Fact]
    public async Task UploadFileAsync_ReturnsDocument()
    {
        var responseBody = new
        {
            objects = new { id = 100, filename = "receipt.pdf", extension = "pdf", mimeType = "application/pdf" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        using var stream = new MemoryStream([0x25, 0x50, 0x44, 0x46]);
        var result = await client.Vouchers.UploadFileAsync(stream, "receipt.pdf");

        result.Id.ShouldBe(100);
        result.Filename.ShouldBe("receipt.pdf");
    }
}
