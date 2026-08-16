using System.Net;
using sevDesk.NET.Exceptions;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

/// <summary>
/// The <c>Factory</c> endpoints are wrapped by methods that post, parse an identifier and then read
/// the object back. Only the post writes. These tests pin down that the three possible outcomes stay
/// distinguishable for a caller who has to decide whether to send the write again.
/// </summary>
public class FactoryWriteTests
{
    // Keys for the five call sites that share the post → parse → read-back shape.
    public const string Invoice = "Invoice";
    public const string CreditNote = "CreditNote";
    public const string CreditNoteFromInvoice = "CreditNoteFromInvoice";
    public const string Order = "Order";
    public const string Voucher = "Voucher";

    public static TheoryData<string> AllFactoryWrites =>
        [Invoice, CreditNote, CreditNoteFromInvoice, Order, Voucher];

    private static string ExpectedObjectName(string key) =>
        key == CreditNoteFromInvoice ? CreditNote : key;

    private static string FactoryResponseFor(string key) => ExpectedObjectName(key) switch
    {
        Invoice => """{"objects":{"invoice":{"id":99}}}""",
        CreditNote => """{"objects":{"creditNote":{"id":99}}}""",
        Order => """{"objects":{"order":{"id":99}}}""",
        Voucher => """{"objects":{"voucher":{"id":99}}}""",
        var other => throw new ArgumentOutOfRangeException(nameof(key), other, null)
    };

    private static Task SaveAsync(SevDeskClient client, string key) => key switch
    {
        Invoice => client.Invoices.SaveInvoiceAsync(
            new Invoice { InvoiceNumber = "RE-099" },
            [new InvoicePos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        CreditNote => client.CreditNotes.SaveCreditNoteAsync(
            new CreditNote { CreditNoteNumber = "GS-099" },
            [new CreditNotePos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        CreditNoteFromInvoice => client.CreditNotes.CreateFromInvoiceAsync(42),
        Order => client.Orders.SaveOrderAsync(
            new Order { OrderNumber = "AN-099" },
            [new OrderPos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        Voucher => client.Vouchers.SaveVoucherAsync(
            new Voucher { Description = "Voucher 99" },
            [new VoucherPos { Net = 100, TaxRate = 19 }]),
        var other => throw new ArgumentOutOfRangeException(nameof(key), other, null)
    };

    private static Task<SevDeskObjectReference> SaveReferenceAsync(SevDeskClient client, string key) => key switch
    {
        Invoice => client.Invoices.SaveInvoiceReferenceAsync(
            new Invoice { InvoiceNumber = "RE-099" },
            [new InvoicePos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        CreditNote => client.CreditNotes.SaveCreditNoteReferenceAsync(
            new CreditNote { CreditNoteNumber = "GS-099" },
            [new CreditNotePos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        CreditNoteFromInvoice => client.CreditNotes.CreateFromInvoiceReferenceAsync(42),
        Order => client.Orders.SaveOrderReferenceAsync(
            new Order { OrderNumber = "AN-099" },
            [new OrderPos { Name = "Position 1", Quantity = 1, Price = 100 }]),
        Voucher => client.Vouchers.SaveVoucherReferenceAsync(
            new Voucher { Description = "Voucher 99" },
            [new VoucherPos { Net = 100, TaxRate = 19 }]),
        var other => throw new ArgumentOutOfRangeException(nameof(key), other, null)
    };

    private static (SevDeskClient Client, ScriptedHttpMessageHandler Handler) CreateClient(params object[] steps)
    {
        var handler = new ScriptedHttpMessageHandler(steps);
        var client = new SevDeskClient(new HttpClient(handler)
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });
        return (client, handler);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body) =>
        new(status) { Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json") };

    // ---------------------------------------------------------------------
    // Outcome 1: the write itself failed. Nothing was created, a retry is safe.
    // ---------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task Save_WhenPostFails_ThrowsPlainApiExceptionAndDoesNotClaimAWrite(string key)
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.UnprocessableEntity, """{"error":{"message":"contact is required"}}"""));

        var ex = await Should.ThrowAsync<SevDeskValidationException>(() => SaveAsync(client, key));

        ex.ShouldNotBeOfType<SevDeskWriteSucceededException>();
        ex.ShouldNotBeAssignableTo<SevDeskWriteSucceededException>();
        ex.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        // The error body still reaches the caller even though the response is no longer
        // buffered by the handler (HttpCompletionOption.ResponseHeadersRead).
        ex.RawResponse.ShouldNotBeNull().ShouldContain("contact is required");
        ex.Message.ShouldContain("contact is required");
        handler.Requests.Count.ShouldBe(1);
    }

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task SaveReference_WhenPostFails_ThrowsPlainApiExceptionAndDoesNotClaimAWrite(string key)
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.UnprocessableEntity, """{"error":{"message":"contact is required"}}"""));

        var ex = await Should.ThrowAsync<SevDeskValidationException>(() => SaveReferenceAsync(client, key));

        ex.ShouldNotBeAssignableTo<SevDeskWriteSucceededException>();
    }

    [Fact]
    public async Task Save_WhenPostFailsOnTheTransport_ThrowsTheTransportErrorUnwrapped()
    {
        var (client, _) = CreateClient(new HttpRequestException("connection reset"));

        var ex = await Should.ThrowAsync<HttpRequestException>(() => SaveAsync(client, Invoice));

        ex.ShouldNotBeAssignableTo<SevDeskWriteSucceededException>();
    }

    // ---------------------------------------------------------------------
    // Outcome 2: the write succeeded, the read-back failed. The id is known.
    // ---------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task Save_WhenPostSucceedsAndReadBackFails_ReportsTheWriteWithItsId(string key)
    {
        var (client, handler) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(key)),
            Json(HttpStatusCode.InternalServerError, """{"error":{"message":"boom"}}"""));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, key));

        ex.ObjectName.ShouldBe(ExpectedObjectName(key));
        ex.ObjectId.ShouldBe(99);
        ex.IsObjectIdKnown.ShouldBeTrue();
        ex.RawResponse.ShouldBe(FactoryResponseFor(key));
        ex.InnerException.ShouldBeOfType<SevDeskApiException>();
        handler.Requests.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Save_WhenReadBackHitsATransportError_ReportsTheWriteWithItsId()
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(Invoice)),
            new HttpRequestException("connection reset"));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, Invoice));

        ex.ObjectId.ShouldBe(99);
        ex.InnerException.ShouldBeOfType<HttpRequestException>();
    }

    [Fact]
    public async Task Save_WhenReadBackIsCancelled_ReportsTheWriteInsteadOfPropagatingTheCancellation()
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(Invoice)),
            new TaskCanceledException("timeout"));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(
            () => client.Invoices.SaveInvoiceAsync(
                new Invoice { InvoiceNumber = "RE-099" },
                [new InvoicePos { Name = "Position 1", Quantity = 1, Price = 100 }],
                CancellationToken.None));

        ex.ObjectId.ShouldBe(99);
        ex.InnerException.ShouldBeOfType<TaskCanceledException>();
    }

    [Fact]
    public async Task Save_WhenReadBackReturnsNotFound_ReportsTheWriteRatherThanANotFound()
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(Invoice)),
            Json(HttpStatusCode.NotFound, """{"error":{"message":"not found"}}"""));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, Invoice));

        ex.ObjectId.ShouldBe(99);
        ex.InnerException.ShouldBeOfType<SevDeskNotFoundException>();
        // The status code describes the failed read-back, not the write.
        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    // ---------------------------------------------------------------------
    // Outcome 3: the write succeeded, its answer was unusable. The id is unknown.
    // ---------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task Save_WhenPostSucceedsButResponseIsUnparsable_ReportsTheWriteWithoutAnId(string key)
    {
        const string body = """{"objects":{}}""";
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, body));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, key));

        ex.ObjectName.ShouldBe(ExpectedObjectName(key));
        ex.ObjectId.ShouldBeNull();
        ex.IsObjectIdKnown.ShouldBeFalse();
        ex.RawResponse.ShouldBe(body);
        ex.InnerException.ShouldBeOfType<SevDeskApiException>();
        // No read-back was attempted — there was no id to read back with.
        handler.Requests.Count.ShouldBe(1);
    }

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task SaveReference_WhenPostSucceedsButResponseIsUnparsable_ReportsTheWriteWithoutAnId(string key)
    {
        var (client, _) = CreateClient(Json(HttpStatusCode.OK, """{"objects":{}}"""));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveReferenceAsync(client, key));

        ex.ObjectId.ShouldBeNull();
    }

    [Fact]
    public async Task Save_WhenPostSucceedsButResponseIsNotJson_ReportsTheWriteWithoutAnId()
    {
        const string body = "<html>502 Bad Gateway</html>";
        var (client, _) = CreateClient(Json(HttpStatusCode.OK, body));

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, Invoice));

        ex.ObjectId.ShouldBeNull();
        ex.RawResponse.ShouldBe(body);
        ex.InnerException.ShouldBeAssignableTo<System.Text.Json.JsonException>();
    }

    [Fact]
    public async Task Save_WhenTheResponseBodyItselfCannotBeRead_ReportsTheWriteWithoutAnIdOrABody()
    {
        var (client, _) = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ThrowingHttpContent(new IOException("connection closed mid-body"))
        });

        var ex = await Should.ThrowAsync<SevDeskWriteSucceededException>(() => SaveAsync(client, Invoice));

        ex.ObjectId.ShouldBeNull();
        ex.RawResponse.ShouldBeNull();
        ex.Message.ShouldContain("Invoice");
        ex.Message.ShouldContain("could not be determined");
    }

    // ---------------------------------------------------------------------
    // The read-back-free variant.
    // ---------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(AllFactoryWrites))]
    public async Task SaveReference_MakesASingleRequestAndReturnsTheReference(string key)
    {
        var (client, handler) = CreateClient(Json(HttpStatusCode.OK, FactoryResponseFor(key)));

        var reference = await SaveReferenceAsync(client, key);

        reference.Id.ShouldBe(99);
        reference.ObjectName.ShouldBe(ExpectedObjectName(key));
        handler.Requests.Count.ShouldBe(1);
        handler.Requests[0].Method.ShouldBe(HttpMethod.Post);
        handler.Requests[0].RequestUri!.ToString().ShouldContain("/Factory/");
    }

    // ---------------------------------------------------------------------
    // Backwards compatibility: existing catch blocks keep catching.
    // ---------------------------------------------------------------------

    [Fact]
    public void SevDeskWriteSucceededException_InheritsTheTypeCallersAlreadyCatch()
    {
        var ex = new SevDeskWriteSucceededException("Invoice", 99, "{}", new InvalidOperationException());

        ex.ShouldBeAssignableTo<SevDeskApiException>();
        ex.ShouldBeAssignableTo<SevDeskException>();
        ex.ShouldBeAssignableTo<Exception>();
    }

    [Fact]
    public async Task Save_WhenReadBackFails_IsStillCaughtByAPreExistingApiExceptionHandler()
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(Invoice)),
            Json(HttpStatusCode.InternalServerError, "{}"));

        var caught = false;
        try
        {
            await SaveAsync(client, Invoice);
        }
        catch (SevDeskApiException)
        {
            // The catch block a 3.0.0 caller already had.
            caught = true;
        }

        caught.ShouldBeTrue();
    }

    [Fact]
    public async Task Save_WhenReadBackFails_IsStillCaughtByAPreExistingBaseExceptionHandler()
    {
        var (client, _) = CreateClient(
            Json(HttpStatusCode.OK, FactoryResponseFor(Invoice)),
            Json(HttpStatusCode.InternalServerError, "{}"));

        var caught = false;
        try
        {
            await SaveAsync(client, Invoice);
        }
        catch (SevDeskException)
        {
            caught = true;
        }

        caught.ShouldBeTrue();
    }
}
