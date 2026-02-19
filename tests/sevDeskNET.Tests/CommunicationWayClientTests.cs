using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class CommunicationWayClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsCommunicationWays()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, type = "EMAIL", value = "test@example.com" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CommunicationWays.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Type.ShouldBe(Models.Enums.CommunicationWayType.EMAIL);
        result.Items[0].Value.ShouldBe("test@example.com");
    }

    [Fact]
    public async Task GetAsync_ReturnsCommunicationWay()
    {
        var responseBody = new
        {
            objects = new { id = 5, type = "PHONE", value = "+49123456789" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CommunicationWays.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Type.ShouldBe(Models.Enums.CommunicationWayType.PHONE);
        result.Value.ShouldBe("+49123456789");
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCommunicationWay()
    {
        var responseBody = new
        {
            objects = new { id = 10, type = "EMAIL", value = "new@example.com" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CommunicationWays.CreateAsync(new CommunicationWay
        {
            Type = Models.Enums.CommunicationWayType.EMAIL,
            Value = "new@example.com"
        });

        result.Id.ShouldBe(10);
        result.Value.ShouldBe("new@example.com");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedCommunicationWay()
    {
        var responseBody = new
        {
            objects = new { id = 5, type = "PHONE", value = "+49987654321" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CommunicationWays.UpdateAsync(5, new CommunicationWay
        {
            Value = "+49987654321"
        });

        result.Id.ShouldBe(5);
        result.Value.ShouldBe("+49987654321");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CommunicationWays.DeleteAsync(1);
    }
}
