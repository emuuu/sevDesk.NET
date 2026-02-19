using System.Net;
using System.Net.Http.Json;
using sevDesk.NET.Tests.Helpers;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class DocumentClientTests
{
    private static SevDeskClient CreateClient(HttpResponseMessage response) =>
        new(new HttpClient(new MockHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://my.sevdesk.de/api/v1/")
        });

    [Fact]
    public async Task ListAsync_ReturnsDocuments()
    {
        var responseBody = new
        {
            objects = new[]
            {
                new { id = 1, filename = "invoice.pdf", extension = "pdf", mimeType = "application/pdf" }
            },
            total = 1
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Documents.ListAsync();

        result.Items.Count.ShouldBe(1);
        result.Items[0].Filename.ShouldBe("invoice.pdf");
        result.Items[0].Extension.ShouldBe("pdf");
    }

    [Fact]
    public async Task GetAsync_ReturnsDocument()
    {
        var responseBody = new
        {
            objects = new { id = 5, filename = "receipt.pdf", extension = "pdf", mimeType = "application/pdf" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        var result = await client.Documents.GetAsync(5);

        result.Id.ShouldBe(5);
        result.Filename.ShouldBe("receipt.pdf");
    }

    [Fact]
    public async Task UploadAsync_ReturnsDocument()
    {
        var responseBody = new
        {
            objects = new { id = 20, filename = "upload.pdf", extension = "pdf", mimeType = "application/pdf" }
        };

        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });

        using var stream = new MemoryStream([0x25, 0x50, 0x44, 0x46]);
        var result = await client.Documents.UploadAsync(stream, "upload.pdf");

        result.Id.ShouldBe(20);
        result.Filename.ShouldBe("upload.pdf");
    }

    [Fact]
    public async Task DownloadAsync_ReturnsBytes()
    {
        var fileBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(fileBytes)
        });

        var result = await client.Documents.DownloadAsync(5);

        result.ShouldBe(fileBytes);
    }
}
