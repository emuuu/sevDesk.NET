using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class AccountingContactClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    private static (SevDeskClient Client, MockHttpMessageHandler Handler) CreateClientWithHandler(HttpResponseMessage response)
    {
        var handler = new MockHttpMessageHandler(response);
        return (new SevDeskClient(new HttpClient(handler) { BaseAddress = new Uri("https://my.sevdesk.de/api/v1/") }), handler);
    }

    [Fact]
    public async Task ListAsync_ReturnsAccountingContacts()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1001, contactName = "Test GmbH", debitorNumber = "10042" },
                new { id = 1002, contactName = "Muster AG", debitorNumber = "10043" }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.AccountingContacts.ListAsync();

        result.Total.ShouldBe(2);
        result.Items.Count.ShouldBe(2);
        result.Items[0].ContactName.ShouldBe("Test GmbH");
        result.Items[0].DebitorNumber.ShouldBe("10042");
        result.Items[1].DebitorNumber.ShouldBe("10043");
    }

    [Fact]
    public async Task ListAsync_WithContactId_AddsContactFilterToQuery()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.AccountingContacts.ListAsync(contactId: 12345678);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.ShouldContain("contact%5Bid%5D=12345678");
        query.ShouldContain("contact%5BobjectName%5D=Contact");
    }

    [Fact]
    public async Task ListAsync_WithoutContactId_OmitsContactFilter()
    {
        var responseBody = new { objects = Array.Empty<object>(), total = 0 };
        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.AccountingContacts.ListAsync();

        handler.LastRequest!.RequestUri!.Query.ShouldNotContain("contact%5Bid%5D");
    }

    [Fact]
    public async Task GetAsync_ReturnsAccountingContact()
    {
        var responseBody = new
        {
            objects = new { id = 1001, contactName = "Test GmbH", debitorNumber = "10042", creditorNumber = "70001" }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.AccountingContacts.GetAsync(1001);

        handler.LastRequest!.RequestUri!.AbsolutePath.ShouldEndWith("/AccountingContact/1001");
        result.Id.ShouldBe(1001);
        result.ContactName.ShouldBe("Test GmbH");
        result.DebitorNumber.ShouldBe("10042");
        result.CreditorNumber.ShouldBe("70001");
    }
}
