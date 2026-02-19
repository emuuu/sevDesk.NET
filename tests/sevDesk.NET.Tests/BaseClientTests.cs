using sevDesk.NET.Exceptions;
using sevDesk.NET.Internal;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class BaseClientTests
{
    [Fact]
    public void ParseFactoryResponseId_NumericId_ReturnsInt()
    {
        var json = """{"objects":{"invoice":{"id":42}}}""";

        var result = BaseClient.ParseFactoryResponseId(json, "invoice");

        result.ShouldBe(42);
    }

    [Fact]
    public void ParseFactoryResponseId_StringId_ReturnsInt()
    {
        var json = """{"objects":{"invoice":{"id":"99"}}}""";

        var result = BaseClient.ParseFactoryResponseId(json, "invoice");

        result.ShouldBe(99);
    }

    [Fact]
    public void ParseFactoryResponseId_MissingProperty_Throws()
    {
        var json = """{"objects":{}}""";

        Should.Throw<SevDeskApiException>(
            () => BaseClient.ParseFactoryResponseId(json, "invoice"));
    }

    [Fact]
    public void ParseFactoryResponseId_InvalidId_Throws()
    {
        var json = """{"objects":{"invoice":{"id":"not-a-number"}}}""";

        Should.Throw<SevDeskApiException>(
            () => BaseClient.ParseFactoryResponseId(json, "invoice"));
    }
}
