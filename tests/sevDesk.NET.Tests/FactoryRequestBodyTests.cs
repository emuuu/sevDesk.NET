using System.Net;
using System.Text.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

/// <summary>
/// The request bodies sent to the <c>Factory</c> save endpoints, checked against the fields the
/// sevDesk API requires. A body with <c>"id":0</c> and without <c>objectName</c>/<c>mapAll</c>
/// is rejected by the live API with 400.
/// </summary>
public class FactoryRequestBodyTests
{
    private const string FactoryResponse = """{"objects":{"invoice":{"id":99}}}""";

    private static (SevDeskClient Client, RecordingHttpMessageHandler Handler) CreateClient(params HttpResponseMessage[] responses)
    {
        var handler = new RecordingHttpMessageHandler(responses);
        var client = new SevDeskClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });
        return (client, handler);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body) =>
        new(status) { Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json") };

    private static Invoice NewInvoice(int id = 0) => new()
    {
        Id = id,
        Contact = new SevDeskObjectReference { Id = 1234, ObjectName = "Contact" },
        InvoiceDate = new DateTime(2023, 4, 24),
        InvoiceType = InvoiceType.RE,
        Status = InvoiceStatus.Draft,
        Currency = "EUR",
        TaxType = "default",
        TaxRate = 19,
        TimeToPay = 30,
        DiscountTime = 14,
        Discount = 3
    };

    private static InvoicePos[] NewPositions(int firstId = 0) =>
    [
        new InvoicePos { Id = firstId, Name = "Sichtprüfung", Quantity = 542, Price = 1.25m, TaxRate = 19, Unity = new SevDeskObjectReference { Id = 1, ObjectName = "Unity" } },
        new InvoicePos { Name = "Prüfung", Quantity = 10, Price = 2.1m, TaxRate = 19, Discount = 20, Unity = new SevDeskObjectReference { Id = 1, ObjectName = "Unity" } }
    ];

    /// <summary>
    /// The complete body for <see cref="NewInvoice"/> and <see cref="NewPositions"/>. Non-ASCII
    /// characters are put in as the <c>\u</c> escapes System.Text.Json writes, via
    /// <see cref="string.Replace(string, string)"/> because C# would resolve them in the literal.
    /// </summary>
    private static readonly string NewInvoiceBody =
        """{"invoice":{"objectName":"Invoice","mapAll":true,"contact":{"id":1234,"objectName":"Contact"},"invoiceDate":"2023-04-24 00:00:00","status":100,"invoiceType":"RE","timeToPay":30,"discountTime":14,"discount":3,"currency":"EUR","taxType":"default","taxRate":19},"invoicePosSave":[{"objectName":"InvoicePos","quantity":542,"price":1.25,"name":"Sichtprüfung","unity":{"id":1,"objectName":"Unity"},"taxRate":19,"mapAll":true},{"objectName":"InvoicePos","quantity":10,"price":2.1,"name":"Prüfung","unity":{"id":1,"objectName":"Unity"},"taxRate":19,"discount":20,"mapAll":true}]}"""
            .Replace("ü", "\\u00FC");

    [Fact]
    public async Task SaveInvoice_NewInvoice_SendsTheSpecConformantBody()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(NewInvoice(), NewPositions());

        handler.Requests[0].Body.ShouldBe(NewInvoiceBody);
    }

    [Fact]
    public async Task SaveInvoice_NewInvoice_SendsObjectNameAndMapAllButNoId()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(NewInvoice(), NewPositions());

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var invoice = body.RootElement.GetProperty("invoice");
        invoice.TryGetProperty("id", out _).ShouldBeFalse();
        invoice.GetProperty("objectName").GetString().ShouldBe("Invoice");
        invoice.GetProperty("mapAll").GetBoolean().ShouldBeTrue();
        foreach (var position in body.RootElement.GetProperty("invoicePosSave").EnumerateArray())
        {
            position.TryGetProperty("id", out _).ShouldBeFalse();
            position.GetProperty("objectName").GetString().ShouldBe("InvoicePos");
            position.GetProperty("mapAll").GetBoolean().ShouldBeTrue();
        }
    }

    [Fact]
    public async Task SaveInvoice_ExistingInvoice_SendsTheRealIds()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(NewInvoice(id: 4711), NewPositions(firstId: 815));

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var invoice = body.RootElement.GetProperty("invoice");
        invoice.GetProperty("id").GetInt32().ShouldBe(4711);
        invoice.GetProperty("objectName").GetString().ShouldBe("Invoice");
        var positions = body.RootElement.GetProperty("invoicePosSave");
        positions[0].GetProperty("id").GetInt32().ShouldBe(815);
        positions[1].TryGetProperty("id", out _).ShouldBeFalse();
    }

    [Fact]
    public async Task InvoiceCreateAndUpdate_DoNotSendTheFactoryOnlyFields()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, """{"objects":{"id":1}}"""),
            Json(HttpStatusCode.OK, """{"objects":{"id":1}}"""));

        await client.Invoices.CreateAsync(NewInvoice());
        await client.Invoices.UpdateAsync(1, NewInvoice(id: 1));

        using var created = JsonDocument.Parse(handler.Requests[0].Body!);
        using var updated = JsonDocument.Parse(handler.Requests[1].Body!);
        created.RootElement.TryGetProperty("id", out _).ShouldBeFalse();
        updated.RootElement.GetProperty("id").GetInt32().ShouldBe(1);
        foreach (var root in new[] { created.RootElement, updated.RootElement })
        {
            root.TryGetProperty("objectName", out _).ShouldBeFalse();
            root.TryGetProperty("mapAll", out _).ShouldBeFalse();
        }
    }
}
