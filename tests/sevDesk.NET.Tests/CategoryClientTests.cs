using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class CategoryClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsCategories()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Lieferant", objectType = "Contact", priority = 100 },
                new { id = 2, name = "Kunde", objectType = "Contact", priority = 200 }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Categories.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].Name.ShouldBe("Lieferant");
        result.Items[0].ObjectType.ShouldBe("Contact");
    }

    [Fact]
    public async Task GetAsync_ReturnsCategory()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Partner", objectType = "Contact" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Categories.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Partner");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCategory()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "New Category", objectType = "Contact" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Categories.CreateAsync(new Category
        {
            Name = "New Category",
            ObjectType = "Contact"
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("New Category");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedCategory()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Updated Category", objectType = "Contact" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Categories.UpdateAsync(5, new Category
        {
            Name = "Updated Category"
        });

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Updated Category");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Categories.DeleteAsync(1);
    }
}
