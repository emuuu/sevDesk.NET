using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class CreditNoteClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
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
}
