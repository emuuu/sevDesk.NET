using System.Net;
using System.Net.Http.Json;
using sevDeskNET.Models;
using sevDeskNET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDeskNET.Tests;

public class CreditNotePosClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsCreditNotePositions()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, name = "Rückgabe Widget", quantity = 1.00m, price = 9.99m }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotePositions.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Name.ShouldBe("Rückgabe Widget");
    }

    [Fact]
    public async Task GetAsync_ReturnsCreditNotePosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Credit Position", quantity = 2.00m, price = 50.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotePositions.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Credit Position");
        result.Price.ShouldBe(50.00m);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCreditNotePosition()
    {
        var responseBody = new
        {
            objects = new { id = 10, name = "New CN Pos", quantity = 1.00m, price = 30.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotePositions.CreateAsync(new CreditNotePos
        {
            Name = "New CN Pos",
            Quantity = 1,
            Price = 30.00m
        });

        result.Id.ShouldBe(10);
        result.Name.ShouldBe("New CN Pos");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedCreditNotePosition()
    {
        var responseBody = new
        {
            objects = new { id = 5, name = "Updated CN Pos", quantity = 3.00m, price = 60.00m }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CreditNotePositions.UpdateAsync(5, new CreditNotePos
        {
            Name = "Updated CN Pos"
        });

        result.Id.ShouldBe(5);
        result.Name.ShouldBe("Updated CN Pos");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CreditNotePositions.DeleteAsync(1);
    }
}
