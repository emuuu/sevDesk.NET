using System.Net;
using System.Text.Json;
using sevDesk.NET.Clients;
using sevDesk.NET.Exceptions;
using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

/// <summary>
/// Document-level discounts (sevDesk object <c>Discounts</c>), as opposed to the early-payment
/// discount of an invoice (<see cref="Invoice.Discount"/>) and the discount of a position
/// (<see cref="InvoicePos.Discount"/>). The response shapes are those recorded from
/// <c>GET /Invoice/{id}?embed=discounts</c> and <c>GET /Invoice/{id}/getDiscounts</c>.
/// </summary>
public class InvoiceDiscountTests
{
    private const string FactoryResponse = """{"objects":{"invoice":{"id":99}}}""";

    private const string KundenrabattJson = """
        {"id":"2263896","objectName":"Discounts","additionalInformation":null,
         "create":"2023-04-24T07:59:09+02:00","update":"2023-04-24T07:59:09+02:00",
         "object":{"id":"52359836","objectName":"Invoice"},"sevClient":"1",
         "discount":"1","text":"Kundenrabatt","percentage":"1","value":"3","isNet":"1"}
        """;

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

    /// <summary>
    /// An invoice with an early-payment discount on the invoice, a discount on one
    /// position, and nothing else discount-related.
    /// </summary>
    private static Invoice SampleInvoice() => new()
    {
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

    private static InvoicePos[] SamplePositions() =>
    [
        new InvoicePos { Name = "Sichtprüfung", Quantity = 542, Price = 1.25m, TaxRate = 19, Unity = new SevDeskObjectReference { Id = 1, ObjectName = "Unity" } },
        new InvoicePos { Name = "Prüfung", Quantity = 10, Price = 2.1m, TaxRate = 19, Discount = 20, Unity = new SevDeskObjectReference { Id = 1, ObjectName = "Unity" } }
    ];

    private static DocumentDiscount Kundenrabatt() => new() { Text = "Kundenrabatt", IsPercentage = true, Value = 3 };

    /// <summary>
    /// The request body without discounts for <see cref="SampleInvoice"/> and
    /// <see cref="SamplePositions"/>, as <see cref="FactoryRequestBodyTests"/> pins it down.
    /// Non-ASCII characters are put in as the <c>\u</c> escapes System.Text.Json writes, via
    /// <see cref="string.Replace(string, string)"/> because C# would resolve them in the literal.
    /// </summary>
    private static readonly string RequestBodyWithoutDiscounts =
        """{"invoice":{"objectName":"Invoice","mapAll":true,"contact":{"id":1234,"objectName":"Contact"},"invoiceDate":"2023-04-24 00:00:00","status":100,"invoiceType":"RE","timeToPay":30,"discountTime":14,"discount":3,"currency":"EUR","taxType":"default","taxRate":19},"invoicePosSave":[{"objectName":"InvoicePos","quantity":542,"price":1.25,"name":"Sichtprüfung","unity":{"id":1,"objectName":"Unity"},"taxRate":19,"mapAll":true},{"objectName":"InvoicePos","quantity":10,"price":2.1,"name":"Prüfung","unity":{"id":1,"objectName":"Unity"},"taxRate":19,"discount":20,"mapAll":true}]}"""
            .Replace("ü", "\\u00FC");

    // ---------------------------------------------------------------------
    // Reading
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAsync_WithEmbeddedDiscounts_ReadsDiscountSumsAndDiscounts()
    {
        // E20231619: positions 1,365.00, customer discount -3 % (40.95), net 1,324.05.
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, $$"""
            {"objects":[{"id":"52359836","objectName":"Invoice","invoiceNumber":"E20231619",
             "discountTime":"14","discount":"3","currency":"EUR",
             "sumNet":"1324.05","sumTax":"251.57","sumGross":"1575.62",
             "sumDiscounts":"-40.95","sumDiscountsForeignCurrency":"0",
             "sumDiscountNet":"-40.95","sumDiscountGross":"-48.74",
             "sumDiscountNetForeignCurrency":"0","sumDiscountGrossForeignCurrency":"0",
             "showNet":"1","discounts":[{{KundenrabattJson}}]}]}
            """));

        var invoice = await client.Invoices.GetAsync(52359836, embed: "discounts");

        handler.Requests[0].Uri!.PathAndQuery.ShouldBe("/api/v1/Invoice/52359836?embed=discounts");
        invoice.SumNet.ShouldBe(1324.05m);
        invoice.SumDiscounts.ShouldBe(-40.95m);
        invoice.SumDiscountNet.ShouldBe(-40.95m);
        invoice.SumDiscountGross.ShouldBe(-48.74m);

        var discount = invoice.Discounts.ShouldHaveSingleItem();
        discount.Id.ShouldBe(2263896);
        discount.Object.ShouldNotBeNull();
        discount.Object.Id.ShouldBe(52359836);
        discount.Object.ObjectName.ShouldBe("Invoice");
        discount.Text.ShouldBe("Kundenrabatt");
        discount.IsPercentage.ShouldBeTrue();
        discount.Value.ShouldBe(3m);
        discount.IsSurcharge.ShouldBeFalse();
        discount.IsNet.ShouldBe(true);
        discount.Create.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAsync_WithoutEmbed_LeavesDiscountsNull()
    {
        var (client, _) = CreateClient(Json(HttpStatusCode.OK,
            """{"objects":[{"id":"1","sumDiscounts":"0","sumDiscountNet":"0","sumDiscountGross":"0"}]}"""));

        var invoice = await client.Invoices.GetAsync(1);

        invoice.Discounts.ShouldBeNull();
        invoice.SumDiscounts.ShouldBe(0m);
    }

    [Fact]
    public async Task GetDiscountsAsync_ReadsTheDiscountsOfTheInvoice()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK,
            $$"""{"objects":[{{KundenrabattJson}}],"total":null}"""));

        var discounts = await client.Invoices.GetDiscountsAsync(52359836);

        handler.Requests[0].Method.ShouldBe(HttpMethod.Get);
        handler.Requests[0].Uri!.AbsolutePath.ShouldBe("/api/v1/Invoice/52359836/getDiscounts");
        var discount = discounts.ShouldHaveSingleItem();
        discount.Text.ShouldBe("Kundenrabatt");
        discount.IsPercentage.ShouldBeTrue();
        discount.Value.ShouldBe(3m);
    }

    [Fact]
    public async Task GetDiscountsAsync_InvoiceWithoutDiscounts_ReturnsEmpty()
    {
        var (client, _) = CreateClient(Json(HttpStatusCode.OK, """{"objects":[],"total":null}"""));

        var discounts = await client.Invoices.GetDiscountsAsync(61404832);

        discounts.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetDiscountsAsync_Surcharge_IsReadAsSurcharge()
    {
        var (client, _) = CreateClient(Json(HttpStatusCode.OK, """
            {"objects":[{"id":"7","objectName":"Discounts","discount":"0","text":"Mindermengenzuschlag","percentage":"0","value":"25.5","isNet":"1"}]}
            """));

        var discount = (await client.Invoices.GetDiscountsAsync(1)).ShouldHaveSingleItem();

        discount.IsSurcharge.ShouldBeTrue();
        discount.IsPercentage.ShouldBeFalse();
        discount.Value.ShouldBe(25.5m);
    }

    // ---------------------------------------------------------------------
    // Writing
    // ---------------------------------------------------------------------

    [Fact]
    public async Task SaveInvoiceReferenceAsync_WithPercentageDiscount_SendsDiscountSave()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        var reference = await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]);

        reference.Id.ShouldBe(99);
        handler.Requests.Count.ShouldBe(1);
        handler.Requests[0].Uri!.AbsolutePath.ShouldBe("/api/v1/Invoice/Factory/saveInvoice");

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var entry = body.RootElement.GetProperty("discountSave").EnumerateArray().ShouldHaveSingleItem();
        entry.GetProperty("discount").GetBoolean().ShouldBeTrue();
        entry.GetProperty("text").GetString().ShouldBe("Kundenrabatt");
        entry.GetProperty("percentage").GetBoolean().ShouldBeTrue();
        entry.GetProperty("value").GetDecimal().ShouldBe(3m);
        entry.GetProperty("objectName").GetString().ShouldBe("Discounts");
        entry.GetProperty("mapAll").GetBoolean().ShouldBeTrue();
        entry.TryGetProperty("isNet", out _).ShouldBeFalse();
        entry.TryGetProperty("id", out _).ShouldBeFalse();
    }

    [Fact]
    public async Task SaveInvoiceReferenceAsync_WithAbsoluteSurcharge_SendsDiscountFalseAndPercentageFalse()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(),
            [new DocumentDiscount { Text = "Mindermengenzuschlag", IsSurcharge = true, IsPercentage = false, Value = 25.5m }]);

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var entry = body.RootElement.GetProperty("discountSave")[0];
        entry.GetProperty("discount").GetBoolean().ShouldBeFalse();
        entry.GetProperty("percentage").GetBoolean().ShouldBeFalse();
        entry.GetProperty("value").GetDecimal().ShouldBe(25.5m);
    }

    [Fact]
    public async Task SaveInvoiceAsync_WithDiscount_SendsDiscountSaveAndReadsTheInvoiceBack()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponse),
            Json(HttpStatusCode.OK, """{"objects":[{"id":"99","sumDiscounts":"-40.95","sumDiscountNet":"-40.95","sumDiscountGross":"-48.74"}]}"""));

        var invoice = await client.Invoices.SaveInvoiceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]);

        handler.Requests.Count.ShouldBe(2);
        handler.Requests[0].Body!.ShouldContain("\"discountSave\":[");
        handler.Requests[1].Method.ShouldBe(HttpMethod.Get);
        invoice.Id.ShouldBe(99);
        invoice.SumDiscountNet.ShouldBe(-40.95m);
    }

    [Fact]
    public async Task SaveInvoiceAsync_WithDiscount_WhenReadBackFails_ReportsTheWrite()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponse),
            Json(HttpStatusCode.InternalServerError, """{"error":{"message":"boom"}}"""));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(
            () => client.Invoices.SaveInvoiceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]));

        ex.ObjectName.ShouldBe("Invoice");
        ex.ObjectId.ShouldBe(99);
        handler.Requests.Count.ShouldBe(2);
    }

    [Fact]
    public async Task SaveInvoiceAsync_WithDiscount_WhenPostFails_DoesNotClaimAWrite()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.BadRequest, """{"error":{"message":"discountSave invalid"}}"""));

        var ex = await Should.ThrowAsync<SevDeskApiException>(
            () => client.Invoices.SaveInvoiceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]));

        ex.ShouldNotBeOfType<SevDeskWriteSucceededException>();
        handler.Requests.Count.ShouldBe(1);
    }

    // ---------------------------------------------------------------------
    // A save without discounts sends exactly the body without discountSave
    // ---------------------------------------------------------------------

    [Fact]
    public async Task SaveInvoiceReferenceAsync_WithoutDiscounts_SendsNoDiscountSave()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions());

        handler.Requests[0].Body.ShouldBe(RequestBodyWithoutDiscounts);
    }

    [Fact]
    public async Task SaveInvoiceAsync_WithoutDiscounts_SendsNoDiscountSave()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponse),
            Json(HttpStatusCode.OK, """{"objects":[{"id":"99"}]}"""));

        await client.Invoices.SaveInvoiceAsync(SampleInvoice(), SamplePositions());

        handler.Requests[0].Body.ShouldBe(RequestBodyWithoutDiscounts);
    }

    [Fact]
    public async Task SaveInvoiceReferenceAsync_WithNullOrEmptyDiscounts_SendsNoDiscountSave()
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponse),
            Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(), null);
        await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(), []);

        handler.Requests[0].Body.ShouldBe(RequestBodyWithoutDiscounts);
        handler.Requests[1].Body.ShouldBe(RequestBodyWithoutDiscounts);
    }

    // ---------------------------------------------------------------------
    // The three kinds of discount stay apart
    // ---------------------------------------------------------------------

    [Fact]
    public async Task Save_EarlyPaymentPositionAndDocumentDiscount_AreSentToTheirOwnFields()
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponse));

        await client.Invoices.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]);

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        var root = body.RootElement;

        // Early-payment discount ("14 Tage 3 % Skonto"): invoice.discount + invoice.discountTime.
        root.GetProperty("invoice").GetProperty("discount").GetDecimal().ShouldBe(3m);
        root.GetProperty("invoice").GetProperty("discountTime").GetInt32().ShouldBe(14);

        // Position discount: invoicePosSave[].discount, only on the position that has one.
        root.GetProperty("invoicePosSave")[0].TryGetProperty("discount", out _).ShouldBeFalse();
        root.GetProperty("invoicePosSave")[1].GetProperty("discount").GetDecimal().ShouldBe(20m);

        // Document-level discount: its own array, not folded into the invoice or a position.
        root.GetProperty("discountSave")[0].GetProperty("text").GetString().ShouldBe("Kundenrabatt");
        root.GetProperty("invoice").TryGetProperty("discounts", out _).ShouldBeFalse();
        root.GetProperty("invoice").TryGetProperty("sumDiscounts", out _).ShouldBeFalse();

        // Everything except discountSave is the request without discounts.
        var withoutDiscountSave = handler.Requests[0].Body!.Replace(
            root.GetProperty("discountSave").GetRawText(), "").Replace(",\"discountSave\":", "");
        withoutDiscountSave.ShouldBe(RequestBodyWithoutDiscounts);
    }

    [Fact]
    public async Task Read_EarlyPaymentPositionAndDocumentDiscount_AreReadIntoTheirOwnProperties()
    {
        var (client, _) = CreateClient(Json(HttpStatusCode.OK, $$"""
            {"objects":[{"id":"52359836","discountTime":"14","discount":"3",
             "sumDiscounts":"-40.95","sumDiscountNet":"-40.95","sumDiscountGross":"-48.74",
             "positions":[{"id":"1","name":"Prüfung","quantity":"10","price":"2.1","discount":"20","taxRate":"19"}],
             "discounts":[{{KundenrabattJson}}]}]}
            """));

        var invoice = await client.Invoices.GetAsync(52359836, embed: "positions,discounts");

        invoice.Discount.ShouldBe(3m);
        invoice.DiscountTime.ShouldBe(14);
        invoice.Positions.ShouldHaveSingleItem().Discount.ShouldBe(20m);
        invoice.Discounts.ShouldHaveSingleItem().Value.ShouldBe(3m);
        invoice.SumDiscountNet.ShouldBe(-40.95m);
    }

    // ---------------------------------------------------------------------
    // Implementations written against 3.1.0 keep compiling
    // ---------------------------------------------------------------------

    /// <summary>
    /// Implements exactly the members <see cref="IInvoiceClient"/> had in 3.1.0 and nothing else,
    /// as a consumer's test fake would. That this class compiles is the first half of the test.
    /// </summary>
    private sealed class InvoiceClient310Fake : IInvoiceClient
    {
        public List<(string Method, Invoice Invoice, IEnumerable<InvoicePos> Positions, CancellationToken Ct)> Calls { get; } = [];

        public Task<Invoice> SaveInvoiceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default)
        {
            Calls.Add((nameof(SaveInvoiceAsync), invoice, positions, ct));
            return Task.FromResult(new Invoice { Id = 7 });
        }

        public Task<SevDeskObjectReference> SaveInvoiceReferenceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default)
        {
            Calls.Add((nameof(SaveInvoiceReferenceAsync), invoice, positions, ct));
            return Task.FromResult(new SevDeskObjectReference { Id = 7, ObjectName = "Invoice" });
        }

        public Task<SevDeskListResponse<Invoice>> ListAsync(PaginationParameters? pagination = null, string? embed = null, InvoiceListFilter? filter = null, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Invoice> GetAsync(int id, string? embed = null, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Invoice> CreateAsync(Invoice invoice, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Invoice> UpdateAsync(int id, Invoice invoice, CancellationToken ct = default) => throw new NotImplementedException();
        public Task DeleteAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task ChangeStatusAsync(int id, InvoiceStatus status, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Invoice> DuplicateAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task CancelAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task MarkAsSentAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task BookAmountAsync(int id, decimal amount, int checkAccountId, DateTime date, CancellationToken ct = default) => throw new NotImplementedException();
    }

    [Fact]
    public async Task Implementation310_SaveWithNullOrEmptyDiscounts_ForwardsToTheOverloadWithoutDiscounts()
    {
        var fake = new InvoiceClient310Fake();
        IInvoiceClient client = fake;
        using var cts = new CancellationTokenSource();
        var invoice = SampleInvoice();
        var positions = SamplePositions();

        (await client.SaveInvoiceAsync(invoice, positions, null, cts.Token)).Id.ShouldBe(7);
        (await client.SaveInvoiceAsync(invoice, positions, [], cts.Token)).Id.ShouldBe(7);
        (await client.SaveInvoiceReferenceAsync(invoice, positions, null, cts.Token)).Id.ShouldBe(7);
        (await client.SaveInvoiceReferenceAsync(invoice, positions, [], cts.Token)).Id.ShouldBe(7);

        fake.Calls.Select(c => c.Method).ShouldBe(["SaveInvoiceAsync", "SaveInvoiceAsync", "SaveInvoiceReferenceAsync", "SaveInvoiceReferenceAsync"]);
        foreach (var call in fake.Calls)
        {
            call.Invoice.ShouldBeSameAs(invoice);
            call.Positions.ShouldBeSameAs(positions);
            call.Ct.ShouldBe(cts.Token);
            call.Ct.CanBeCanceled.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Implementation310_SaveWithDiscounts_ThrowsNotSupportedAndDoesNotSaveWithoutThem()
    {
        var fake = new InvoiceClient310Fake();
        IInvoiceClient client = fake;

        var ex = await Should.ThrowAsync<NotSupportedException>(
            () => client.SaveInvoiceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]));
        await Should.ThrowAsync<NotSupportedException>(
            () => client.SaveInvoiceReferenceAsync(SampleInvoice(), SamplePositions(), [Kundenrabatt()]));

        ex.Message.ShouldContain("does not support document-level discounts");
        fake.Calls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Implementation310_GetDiscountsAsync_ThrowsNotSupported()
    {
        IInvoiceClient client = new InvoiceClient310Fake();

        await Should.ThrowAsync<NotSupportedException>(() => client.GetDiscountsAsync(1));
    }
}

