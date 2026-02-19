using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Models;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class CheckAccountTransactionClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsTransactions()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, amount = 500.00m, payeePayerName = "Max Mustermann", paymtPurpose = "Miete" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccountTransactions.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].PayeeName.ShouldBe("Max Mustermann");
        result.Items[0].Purpose.ShouldBe("Miete");
    }

    [Fact]
    public async Task GetAsync_ReturnsTransaction()
    {
        var responseBody = new
        {
            objects = new { id = 5, amount = 250.50m, payeePayerName = "Test", paymtPurpose = "Payment" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccountTransactions.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Amount.ShouldBe(250.50m);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedTransaction()
    {
        var responseBody = new
        {
            objects = new { id = 10, amount = 100.00m, payeePayerName = "New Payee" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccountTransactions.CreateAsync(new CheckAccountTransaction
        {
            Amount = 100.00m,
            PayeeName = "New Payee"
        });

        result.Id.ShouldBe(10);
        result.PayeeName.ShouldBe("New Payee");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedTransaction()
    {
        var responseBody = new
        {
            objects = new { id = 5, amount = 300.00m, payeePayerName = "Updated Payee" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.CheckAccountTransactions.UpdateAsync(5, new CheckAccountTransaction
        {
            Amount = 300.00m,
            PayeeName = "Updated Payee"
        });

        result.Id.ShouldBe(5);
        result.PayeeName.ShouldBe("Updated Payee");
    }

    [Fact]
    public async Task DeleteAsync_NoError()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));
        await client.CheckAccountTransactions.DeleteAsync(1);
    }
}
