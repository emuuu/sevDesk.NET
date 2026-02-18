using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Exceptions;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class InvoiceClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsInvoices()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, invoiceNumber = "RE-001", status = 100 },
                new { id = 2, invoiceNumber = "RE-002", status = 200 }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].InvoiceNumber.ShouldBe("RE-001");
        result.Items[1].Status.ShouldBe(Models.Enums.InvoiceStatus.Open);
    }

    [Fact]
    public async Task GetAsync_ReturnsInvoice()
    {
        var responseBody = new
        {
            objects = new
            {
                id = 42,
                invoiceNumber = "RE-042",
                status = 1000,
                sumNet = "100.00",
                sumGross = "119.00",
                currency = "EUR"
            }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.GetAsync(42);

        result.Id.ShouldBe(42);
        result.InvoiceNumber.ShouldBe("RE-042");
        result.Status.ShouldBe(Models.Enums.InvoiceStatus.Paid);
        result.SumNet.ShouldBe(100.00m);
        result.SumGross.ShouldBe(119.00m);
        result.Currency.ShouldBe("EUR");
    }

    [Fact]
    public async Task GetAsync_ApiError_ThrowsSevDeskApiException()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("""{"error":"Internal error"}""", System.Text.Encoding.UTF8, "application/json")
        });

        var ex = await Should.ThrowAsync<SevDeskApiException>(
            () => client.Invoices.GetAsync(1));

        ex.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.DeleteAsync(1);
    }
}
