using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class TagClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsTags()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "VIP" },
                new { id = 2, name = "Wichtig" }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Tags.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].Name.ShouldBe("VIP");
        result.Items[1].Name.ShouldBe("Wichtig");
    }

    [Fact]
    public async Task GetAsync_ReturnsTag()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Premium" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Tags.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Premium");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedTag()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "New Tag" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Tags.CreateAsync(new Tag
        {
            Name = "New Tag"
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("New Tag");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.Tags.DeleteAsync(1);
    }
}
