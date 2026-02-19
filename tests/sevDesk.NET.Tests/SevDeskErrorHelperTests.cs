using sevDesk.NET.Internal;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class SevDeskErrorHelperTests
{
    [Fact]
    public void TryParseErrorDetail_WithErrorMessage_ReturnsMessage()
    {
        var json = """{"error":{"message":"Invalid token"}}""";
        SevDeskErrorHelper.TryParseErrorDetail(json).ShouldBe("Invalid token");
    }

    [Fact]
    public void TryParseErrorDetail_WithErrorString_ReturnsString()
    {
        var json = """{"error":"Something went wrong"}""";
        SevDeskErrorHelper.TryParseErrorDetail(json).ShouldBe("Something went wrong");
    }

    [Fact]
    public void TryParseErrorDetail_WithTopLevelMessage_ReturnsMessage()
    {
        var json = """{"message":"Not found"}""";
        SevDeskErrorHelper.TryParseErrorDetail(json).ShouldBe("Not found");
    }

    [Fact]
    public void TryParseErrorDetail_WithInvalidJson_ReturnsRawBody()
    {
        var raw = "not json";
        SevDeskErrorHelper.TryParseErrorDetail(raw).ShouldBe("not json");
    }

    [Fact]
    public void TryParseErrorDetail_WithEmptyJson_ReturnsRawBody()
    {
        var json = "{}";
        SevDeskErrorHelper.TryParseErrorDetail(json).ShouldBe("{}");
    }
}
