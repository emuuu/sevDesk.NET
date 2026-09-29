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

/// <summary>
/// The same defect on the other write paths: the <c>CreditNote</c>, <c>Order</c> and
/// <c>Voucher</c> factories, and the plain REST creates the API documents.
/// </summary>
public class OtherWriteRequestBodyTests
{
    private static (SevDeskClient Client, RecordingHttpMessageHandler Handler) CreateClient(string response)
    {
        var handler = new RecordingHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(response, System.Text.Encoding.UTF8, "application/json") });
        var client = new SevDeskClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });
        return (client, handler);
    }

    private static Task SaveAsync(SevDeskClient client, string objectName, int id, int positionId) => objectName switch
    {
        "CreditNote" => client.CreditNotes.SaveCreditNoteReferenceAsync(
            new CreditNote { Id = id, CreditNoteNumber = "GS-099" },
            [new CreditNotePos { Id = positionId, Name = "Position 1", Quantity = 1, Price = 100 }]),
        "Order" => client.Orders.SaveOrderReferenceAsync(
            new Order { Id = id, OrderNumber = "AN-099" },
            [new OrderPos { Id = positionId, Name = "Position 1", Quantity = 1, Price = 100 }]),
        "Voucher" => client.Vouchers.SaveVoucherReferenceAsync(
            new Voucher { Id = id, Description = "Voucher 99" },
            [new VoucherPos { Id = positionId, Net = 100, TaxRate = 19 }]),
        var other => throw new ArgumentOutOfRangeException(nameof(objectName), other, null)
    };

    private static string Member(string objectName) => char.ToLowerInvariant(objectName[0]) + objectName[1..];

    private static string FactoryResponse(string objectName, int id) =>
        "{\"objects\":{\"" + Member(objectName) + "\":{\"id\":" + id + "}}}";

    [Theory]
    [InlineData("CreditNote")]
    [InlineData("Order")]
    [InlineData("Voucher")]
    public async Task Factory_NewDocument_SendsObjectNameAndMapAllButNoId(string objectName)
    {
        var (client, handler) = CreateClient(FactoryResponse(objectName, 99));

        await SaveAsync(client, objectName, id: 0, positionId: 0);

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var document = body.RootElement.GetProperty(Member(objectName));
        document.TryGetProperty("id", out _).ShouldBeFalse();
        document.GetProperty("objectName").GetString().ShouldBe(objectName);
        document.GetProperty("mapAll").GetBoolean().ShouldBeTrue();
        var position = body.RootElement.GetProperty(Member(objectName) + "PosSave")[0];
        position.TryGetProperty("id", out _).ShouldBeFalse();
        position.GetProperty("objectName").GetString().ShouldBe(objectName + "Pos");
        position.GetProperty("mapAll").GetBoolean().ShouldBeTrue();
    }

    [Theory]
    [InlineData("CreditNote")]
    [InlineData("Order")]
    [InlineData("Voucher")]
    public async Task Factory_ExistingDocument_SendsTheRealIds(string objectName)
    {
        var (client, handler) = CreateClient(FactoryResponse(objectName, 4711));

        await SaveAsync(client, objectName, id: 4711, positionId: 815);

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        body.RootElement.GetProperty(Member(objectName)).GetProperty("id").GetInt32().ShouldBe(4711);
        body.RootElement.GetProperty(Member(objectName) + "PosSave")[0].GetProperty("id").GetInt32().ShouldBe(815);
    }

    [Fact]
    public async Task ContactCreate_SendsNoIdAndTheCategory()
    {
        var (client, handler) = CreateClient("""{"objects":{"id":1,"objectName":"Contact"}}""");

        await client.Contacts.CreateAsync(new Contact
        {
            Name = "Dummy",
            Category = new SevDeskObjectReference { Id = 3, ObjectName = "Category" }
        });

        handler.Requests[0].Uri!.AbsolutePath.ShouldBe("/api/v1/Contact");
        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        body.RootElement.TryGetProperty("id", out _).ShouldBeFalse();
        body.RootElement.GetProperty("name").GetString().ShouldBe("Dummy");
        body.RootElement.GetProperty("category").GetProperty("id").GetInt32().ShouldBe(3);
        body.RootElement.GetProperty("category").GetProperty("objectName").GetString().ShouldBe("Category");
    }

    [Fact]
    public async Task ContactUpdate_SendsTheRealId()
    {
        var (client, handler) = CreateClient("""{"objects":{"id":7,"objectName":"Contact"}}""");

        await client.Contacts.UpdateAsync(7, new Contact { Id = 7, Name = "Dummy" });

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        body.RootElement.GetProperty("id").GetInt32().ShouldBe(7);
    }

    public static TheoryData<string> RestCreates => ["Part", "CommunicationWay", "ContactAddress", "CheckAccountTransaction"];

    [Theory]
    [MemberData(nameof(RestCreates))]
    public async Task RestCreate_NewObject_SendsNoId(string objectName)
    {
        var (client, handler) = CreateClient("""{"objects":{"id":1}}""");

        await (objectName switch
        {
            "Part" => client.Parts.CreateAsync(new Part { Name = "Part" }),
            "CommunicationWay" => (Task)client.CommunicationWays.CreateAsync(new CommunicationWay { Value = "mail@example.com" }),
            "ContactAddress" => client.ContactAddresses.CreateAsync(new ContactAddress { City = "Münster" }),
            "CheckAccountTransaction" => client.CheckAccountTransactions.CreateAsync(new CheckAccountTransaction { Amount = 1 }),
            var other => throw new ArgumentOutOfRangeException(nameof(objectName), other, null)
        });

        handler.Requests[0].Uri!.AbsolutePath.ShouldBe("/api/v1/" + objectName);
        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        body.RootElement.TryGetProperty("id", out _).ShouldBeFalse();
    }
}
