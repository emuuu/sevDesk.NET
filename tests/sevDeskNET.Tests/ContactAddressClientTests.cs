using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class ContactAddressClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsContactAddresses()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, street = "Musterstraße 1", zip = "12345", city = "Berlin" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.ContactAddresses.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Street.ShouldBe("Musterstraße 1");
        result.Items[0].City.ShouldBe("Berlin");
    }

    [Fact]
    public async Task GetAsync_ReturnsContactAddress()
    {
        var responseBody = new
        {
            objects = new { id = 5, street = "Hauptstraße 10", zip = "80333", city = "München" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.ContactAddresses.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Street.ShouldBe("Hauptstraße 10");
        result.Zip.ShouldBe("80333");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedContactAddress()
    {
        var responseBody = new
        {
            objects = new { id = 10, street = "Neue Straße 5", zip = "10115", city = "Berlin" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.ContactAddresses.CreateAsync(new ContactAddress
        {
            Street = "Neue Straße 5",
            Zip = "10115",
            City = "Berlin"
        });

        result.Id.ShouldBe(10);
        result.Street.ShouldBe("Neue Straße 5");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedContactAddress()
    {
        var responseBody = new
        {
            objects = new { id = 5, street = "Updated Straße 20", zip = "80333", city = "München" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.ContactAddresses.UpdateAsync(5, new ContactAddress
        {
            Street = "Updated Straße 20"
        });

        result.Id.ShouldBe(5);
        result.Street.ShouldBe("Updated Straße 20");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.ContactAddresses.DeleteAsync(1);
    }
}
