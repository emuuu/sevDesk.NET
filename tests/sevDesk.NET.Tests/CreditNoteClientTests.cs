using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class CreditNoteClientTests
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
    public async Task ListAsync_ReturnsCreditNotes()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, creditNoteNumber = "GS-001", status = 200 }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotes.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].CreditNoteNumber.ShouldBe("GS-001");
        result.Items[0].Status.ShouldBe(Models.Enums.CreditNoteStatus.Open);
    }

    [Fact]
    public async Task GetAsync_ReturnsCreditNote()
    {
        var responseBody = new
        {
            objects = new { id = 10, creditNoteNumber = "GS-010", status = 100, currency = "EUR" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotes.GetAsync(10);

        result.Id.ShouldBe(10);
        result.CreditNoteNumber.ShouldBe("GS-010");
        result.Currency.ShouldBe("EUR");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCreditNote()
    {
        var responseBody = new
        {
            objects = new { id = 20, creditNoteNumber = "GS-020", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotes.CreateAsync(new CreditNote
        {
            CreditNoteNumber = "GS-020"
        });

        result.Id.ShouldBe(20);
        result.CreditNoteNumber.ShouldBe("GS-020");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedCreditNote()
    {
        var responseBody = new
        {
            objects = new { id = 10, creditNoteNumber = "GS-010-Updated", status = 200 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotes.UpdateAsync(10, new CreditNote
        {
            CreditNoteNumber = "GS-010-Updated"
        });

        result.Id.ShouldBe(10);
        result.CreditNoteNumber.ShouldBe("GS-010-Updated");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CreditNotes.DeleteAsync(1);
    }

    [Fact]
    public async Task SaveCreditNoteAsync_ReturnsCreditNote()
    {
        var factoryResponse = new StringContent(
            """{"objects":{"creditNote":{"id":30}}}""",
            System.Text.Encoding.UTF8, "application/json");

        var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new
            {
                objects = new { id = 30, creditNoteNumber = "GS-030", status = 100 }
            })
        };

        var client = CreateSequentialClient(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = factoryResponse },
            getResponse);

        var result = await client.CreditNotes.SaveCreditNoteAsync(
            new CreditNote { CreditNoteNumber = "GS-030" },
            [new CreditNotePos { Name = "Position 1", Quantity = 1, Price = 50 }]);

        result.Id.ShouldBe(30);
        result.CreditNoteNumber.ShouldBe("GS-030");
    }

    [Fact]
    public async Task GetPdfAsync_ReturnsPdfBytes()
    {
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(pdfBytes)
        });

        var result = await client.CreditNotes.GetPdfAsync(1);

        result.ShouldBe(pdfBytes);
    }

    [Fact]
    public async Task SendViaEmailAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CreditNotes.SendViaEmailAsync(1, "test@example.com", "Credit Note", "See attached");
    }

    [Fact]
    public async Task CreateFromInvoiceAsync_ReturnsCreditNote()
    {
        var factoryResponse = new StringContent(
            """{"objects":{"creditNote":{"id":40}}}""",
            System.Text.Encoding.UTF8, "application/json");

        var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new
            {
                objects = new { id = 40, creditNoteNumber = "GS-040", status = 100 }
            })
        };

        var client = CreateSequentialClient(
            new HttpResponseMessage(HttpStatusCode.OK) { Content = factoryResponse },
            getResponse);

        var result = await client.CreditNotes.CreateFromInvoiceAsync(99);

        result.Id.ShouldBe(40);
        result.CreditNoteNumber.ShouldBe("GS-040");
    }
}
