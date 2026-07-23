using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Exceptions;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class InvoiceClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    private static (SevDeskClient Client, MockHttpMessageHandler Handler) CreateClientWithHandler(HttpResponseMessage response)
    {
        var handler = new MockHttpMessageHandler(response);
        var client = new SevDeskClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });
        return (client, handler);
    }

    private static SevDeskClient CreateSequentialClient(params HttpResponseMessage[] responses) =>
        new(new HttpClient(new SequentialMockHttpMessageHandler(responses))
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

    [Fact]
    public async Task CreateAsync_ReturnsCreatedInvoice()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR"
        });

        result.Id.ShouldBe(50);
        result.InvoiceNumber.ShouldBe("RE-050");
    }

    [Fact]
    public async Task CreateAsync_SingleObjectEnvelope_DeserializesNumericSums()
    {
        // Real POST responses wrap the created object in a single-object "objects" envelope
        // (not a list) and return sumNet/sumGross/sumTax as JSON numbers rather than strings.
        var responseBody = new
        {
            objects = new
            {
                id = 50,
                invoiceNumber = "RE-050",
                status = 100,
                currency = "EUR",
                sumNet = 84.02m,
                sumGross = 99.98m,
                sumTax = 15.96m
            }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR"
        });

        result.SumNet.ShouldBe(84.02m);
        result.SumGross.ShouldBe(99.98m);
        result.SumTax.ShouldBe(15.96m);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedInvoice()
    {
        var responseBody = new
        {
            objects = new { id = 42, invoiceNumber = "RE-042-Updated", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.UpdateAsync(42, new Invoice
        {
            InvoiceNumber = "RE-042-Updated"
        });

        result.Id.ShouldBe(42);
        result.InvoiceNumber.ShouldBe("RE-042-Updated");
    }

    [Fact]
    public async Task UpdateAsync_SingleObjectEnvelope_DeserializesNumericSums()
    {
        // Real PUT responses wrap the updated object in a single-object "objects" envelope
        // (not a list) and return sumNet/sumGross/sumTax as JSON numbers rather than strings.
        var responseBody = new
        {
            objects = new
            {
                id = 42,
                invoiceNumber = "RE-042-Updated",
                status = 100,
                sumNet = 84.02m,
                sumGross = 99.98m,
                sumTax = 15.96m
            }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.UpdateAsync(42, new Invoice
        {
            InvoiceNumber = "RE-042-Updated"
        });

        result.SumNet.ShouldBe(84.02m);
        result.SumGross.ShouldBe(99.98m);
        result.SumTax.ShouldBe(15.96m);
    }

    [Fact]
    public async Task SaveInvoiceAsync_ReturnsInvoice()
    {
        var factoryResponse = new StringContent(
            """{"objects":{"invoice":{"id":99}}}""",
            System.Text.Encoding.UTF8, "application/json");

        var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new
            {
                objects = new { id = 99, invoiceNumber = "RE-099", status = 100 }
            })
        };

        var client = CreateSequentialClient(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = factoryResponse },
            getResponse);

        var result = await client.Invoices.SaveInvoiceAsync(
            new Invoice { InvoiceNumber = "RE-099" },
            [new InvoicePos { Name = "Position 1", Quantity = 1, Price = 100 }]);

        result.Id.ShouldBe(99);
        result.InvoiceNumber.ShouldBe("RE-099");
    }

    [Fact]
    public async Task ChangeStatusAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.ChangeStatusAsync(1, Models.Enums.InvoiceStatus.Open);
    }

    [Fact]
    public async Task GetPdfAsync_ReturnsPdfBytes()
    {
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(pdfBytes)
        });

        var result = await client.Invoices.GetPdfAsync(1);

        result.ShouldBe(pdfBytes);
    }

    [Fact]
    public async Task SendViaEmailAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.SendViaEmailAsync(1, "test@example.com", "Invoice", "Please find attached");
    }

    [Fact]
    public async Task BookAmountAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.BookAmountAsync(1, 119.00m, 5, DateTime.Today);
    }

    [Fact]
    public async Task CancelAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.CancelAsync(1);
    }

    [Fact]
    public async Task DuplicateAsync_ReturnsDuplicatedInvoice()
    {
        var responseBody = new
        {
            objects = new { id = 51, invoiceNumber = "RE-051", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Invoices.DuplicateAsync(50);

        result.Id.ShouldBe(51);
    }

    [Fact]
    public async Task MarkAsSentAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Invoices.MarkAsSentAsync(1);
    }

    [Fact]
    public async Task CreateAsync_SendsPaymentMethodAndTaxRuleAsObjectReferences()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR",
            PaymentMethod = new SevDeskObjectReference { Id = 42, ObjectName = "PaymentMethod" },
            TaxRule = new SevDeskObjectReference { Id = 1, ObjectName = "TaxRule" }
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"paymentMethod\":{\"id\":42,\"objectName\":\"PaymentMethod\"}");
        requestBody.ShouldContain("\"taxRule\":{\"id\":1,\"objectName\":\"TaxRule\"}");
    }

    [Fact]
    public async Task CreateAsync_PropertyIsEInvoiceTrue_SendsPropertyIsEInvoiceAsOne()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR",
            PropertyIsEInvoice = true
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"propertyIsEInvoice\":\"1\"");
    }

    [Fact]
    public async Task CreateAsync_PropertyIsEInvoiceFalse_SendsPropertyIsEInvoiceAsZero()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR",
            PropertyIsEInvoice = false
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"propertyIsEInvoice\":\"0\"");
    }

    [Fact]
    public async Task CreateAsync_PropertyIsEInvoiceNull_OmitsPropertyFromRequestBody()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR",
            PropertyIsEInvoice = null
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldNotContain("propertyIsEInvoice");
    }

    [Fact]
    public async Task CreateAsync_SendsEinvoiceReferenceAsIs()
    {
        var responseBody = new
        {
            objects = new { id = 50, invoiceNumber = "RE-050", status = 100, currency = "EUR" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.CreateAsync(new Invoice
        {
            InvoiceNumber = "RE-050",
            Currency = "EUR",
            EinvoiceReference = "991-33333TEST-33"
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"einvoiceReference\":\"991-33333TEST-33\"");
    }

    [Fact]
    public async Task ListAsync_WithUpdateAfterFilter_AddsUpdateAfterToQuery()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.ListAsync(filter: new InvoiceListFilter
        {
            UpdateAfter = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
        });

        handler.LastRequest!.RequestUri!.Query.ShouldContain("updateAfter=1767225600");
    }

    [Fact]
    public async Task ListAsync_WithStatusFilter_AddsStatusToQuery()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.ListAsync(filter: new InvoiceListFilter { Status = Models.Enums.InvoiceStatus.Open });

        handler.LastRequest!.RequestUri!.Query.ShouldContain("status=200");
    }

    [Fact]
    public async Task ListAsync_WithContactIdFilter_AddsContactIdAndObjectNameToQuery()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.ListAsync(filter: new InvoiceListFilter { ContactId = 12345678 });

        var query = handler.LastRequest!.RequestUri!.Query;
        query.ShouldContain("contact%5Bid%5D=12345678");
        query.ShouldContain("contact%5BobjectName%5D=Contact");
    }

    [Fact]
    public async Task ListAsync_WithCombinedFilter_AddsAllQueryParameters()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.ListAsync(filter: new InvoiceListFilter
        {
            UpdateAfter = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Status = Models.Enums.InvoiceStatus.Open,
            ContactId = 12345678
        });

        var query = handler.LastRequest!.RequestUri!.Query;
        query.ShouldContain("updateAfter=1767225600");
        query.ShouldContain("status=200");
        query.ShouldContain("contact%5Bid%5D=12345678");
        query.ShouldContain("contact%5BobjectName%5D=Contact");
    }

    [Fact]
    public async Task ListAsync_WithoutFilter_OmitsFilterQueryParameters()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Invoices.ListAsync();

        var query = handler.LastRequest!.RequestUri!.Query;
        query.ShouldNotContain("updateAfter");
        query.ShouldNotContain("status");
        query.ShouldNotContain("contact%5Bid%5D");
    }
}
