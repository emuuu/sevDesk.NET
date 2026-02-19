using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class UnityClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsUnities()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Stück", translationCode = "PIECE" },
                new { id = 2, name = "Stunde", translationCode = "HOUR" }
            },
            total = 2
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Unities.ListAsync();

        result.Items.Count.ShouldBe(2);
        result.Items[0].Name.ShouldBe("Stück");
        result.Items[1].Name.ShouldBe("Stunde");
    }

    [Fact]
    public async Task GetAsync_ReturnsUnity()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Kilogramm", translationCode = "KG" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Unities.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Kilogramm");
        result.TranslationCode.ShouldBe("KG");
    }
}
