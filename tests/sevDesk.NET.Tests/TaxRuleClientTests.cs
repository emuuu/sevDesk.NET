using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class TaxRuleClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsTaxRules()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Regelbesteuerung", description = "Standard tax rule", code = "1", countryContactType = "domestic" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.TaxRules.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Name.ShouldBe("Regelbesteuerung");
        result.Items[0].Description.ShouldBe("Standard tax rule");
        result.Items[0].Code.ShouldBe("1");
    }

    [Fact]
    public async Task GetAsync_ReturnsTaxRule()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Ermäßigt", description = "Reduced rate", code = "2", countryContactType = "domestic" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.TaxRules.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Ermäßigt");
        result.Description.ShouldBe("Reduced rate");
    }
}
