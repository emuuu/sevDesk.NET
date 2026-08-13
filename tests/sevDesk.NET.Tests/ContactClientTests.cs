using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Exceptions;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class ContactClientTests
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

    [Fact]
    public async Task ListAsync_ReturnsContacts()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, surename = "Max", familyname = "Mustermann", name = "Test GmbH", status = 100 },
                new { id = 2, surename = "Erika", familyname = "Musterfrau", name = "Muster AG", status = 100 }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Total.ShouldBe(2);
        result.Items[0].Id.ShouldBe(1);
        result.Items[0].Surename.ShouldBe("Max");
        result.Items[0].Familyname.ShouldBe("Mustermann");
    }

    [Fact]
    public async Task ListAsync_MissingTotal_ReturnsNullTotal()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, surename = "Max", familyname = "Mustermann", name = "Test GmbH", status = 100 }
            }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Total.ShouldBeNull(); // no total from the server ≠ a total of 0
    }

    [Fact]
    public async Task ListAsync_ZeroTotal_ReturnsZeroTotal()
    {
        var responseBody = new
        {
            objects = Array.Empty<object>(),
            total = "0"
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.ListAsync();

        result.Items.ShouldBeEmpty();
        result.Total.ShouldBe(0);
    }

    [Fact]
    public async Task GetAsync_ReturnsContact()
    {
        var responseBody = new
        {
            objects = new { id = 42, surename = "Max", familyname = "Mustermann", name = "Test GmbH", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.GetAsync(42);

        result.Id.ShouldBe(42);
        result.Surename.ShouldBe("Max");
    }

    [Fact]
    public async Task GetAsync_NotFound_ThrowsSevDeskNotFoundException()
    {
        var errorBody = """{"error":"Contact not found"}""";
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(errorBody, System.Text.Encoding.UTF8, "application/json")
        });

        var ex = await Should.ThrowAsync<SevDeskNotFoundException>(
            () => client.Contacts.GetAsync(999));

        ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        ex.RawResponse!.ShouldContain("Contact not found");
    }

    [Fact]
    public async Task GetAsync_Unauthorized_ThrowsSevDeskAuthenticationException()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("""{"error":"Unauthorized"}""", System.Text.Encoding.UTF8, "application/json")
        });

        await Should.ThrowAsync<SevDeskAuthenticationException>(
            () => client.Contacts.GetAsync(1));
    }

    [Fact]
    public async Task GetAsync_UnprocessableEntity_ThrowsValidationException()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.UnprocessableEntity)
        {
            Content = new StringContent("""{"error":"Validation failed"}""", System.Text.Encoding.UTF8, "application/json")
        });

        var ex = await Should.ThrowAsync<SevDeskValidationException>(
            () => client.Contacts.GetAsync(1));

        ex.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedContact()
    {
        var responseBody = new
        {
            objects = new { id = 100, surename = "Neu", familyname = "Kontakt", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.CreateAsync(new Contact
        {
            Surename = "Neu",
            Familyname = "Kontakt"
        });

        result.Id.ShouldBe(100);
        result.Surename.ShouldBe("Neu");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedContact()
    {
        var responseBody = new
        {
            objects = new { id = 42, surename = "Updated", familyname = "Contact", status = 100 }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Contacts.UpdateAsync(42, new Contact
        {
            Surename = "Updated",
            Familyname = "Contact"
        });

        result.Id.ShouldBe(42);
        result.Surename.ShouldBe("Updated");
    }

    [Fact]
    public async Task CreateAsync_SendsBuyerReferenceAsIs()
    {
        var responseBody = new
        {
            objects = new { id = 100, surename = "Neu", familyname = "Kontakt", status = 100 }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Contacts.CreateAsync(new Contact
        {
            Surename = "Neu",
            Familyname = "Kontakt",
            BuyerReference = "991-33333TEST-33"
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"buyerReference\":\"991-33333TEST-33\"");
    }

    [Fact]
    public async Task UpdateAsync_SendsBuyerReferenceAsIs()
    {
        var responseBody = new
        {
            objects = new { id = 42, surename = "Updated", familyname = "Contact", status = 100 }
        };

        var (client, handler) = CreateClientWithHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        await client.Contacts.UpdateAsync(42, new Contact
        {
            Surename = "Updated",
            Familyname = "Contact",
            BuyerReference = "991-33333TEST-33"
        });

        var requestBody = await handler.LastRequest!.Content!.ReadAsStringAsync();
        requestBody.ShouldContain("\"buyerReference\":\"991-33333TEST-33\"");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Contacts.DeleteAsync(1);
    }

    [Fact]
    public async Task GetNextCustomerNumberAsync_ReturnsNumber()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"objects":"10042"}""", System.Text.Encoding.UTF8, "application/json")
        });

        var result = await client.Contacts.GetNextCustomerNumberAsync();

        result.ShouldBe("10042");
    }
}
