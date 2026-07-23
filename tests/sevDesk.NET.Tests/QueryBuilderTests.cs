using sevDesk.NET.Internal;
using Shouldly;
using Xunit;

namespace sevDesk.NET.Tests;

public class QueryBuilderTests
{
    [Fact]
    public void Build_WithNoParameters_ReturnsBasePath()
    {
        var qb = new QueryBuilder();
        qb.Build("/Contact").ShouldBe("/Contact");
    }

    [Fact]
    public void Build_WithParameters_AppendsQueryString()
    {
        var qb = new QueryBuilder();
        qb.Add("limit", "100");
        qb.Add("offset", "0");
        qb.Build("/Contact").ShouldBe("/Contact?limit=100&offset=0");
    }

    [Fact]
    public void AddIfNotNull_SkipsNullValues()
    {
        var qb = new QueryBuilder();
        qb.AddIfNotNull("embed", (string?)null);
        qb.Add("limit", "10");
        qb.Build("/Contact").ShouldBe("/Contact?limit=10");
    }

    [Fact]
    public void AddIfNotNull_IncludesNonNullValues()
    {
        var qb = new QueryBuilder();
        qb.AddIfNotNull("embed", "addresses");
        qb.Build("/Contact").ShouldBe("/Contact?embed=addresses");
    }

    [Fact]
    public void AddPagination_AddsLimitOffsetCountAll()
    {
        var qb = new QueryBuilder();
        qb.AddPagination(new PaginationParameters { Limit = 50, Offset = 10 });
        var result = qb.Build("/Invoice");
        result.ShouldContain("limit=50");
        result.ShouldContain("offset=10");
        result.ShouldContain("countAll=true");
    }

    [Fact]
    public void AddPagination_DefaultValues()
    {
        var qb = new QueryBuilder();
        qb.AddPagination(null);
        var result = qb.Build("/Invoice");
        result.ShouldContain("limit=100");
        result.ShouldContain("offset=0");
    }

    [Fact]
    public void AddPagination_ClampsLimitAboveMaxTo2000()
    {
        var qb = new QueryBuilder();
        qb.AddPagination(new PaginationParameters { Limit = 5000 });
        qb.Build("/Invoice").ShouldContain("limit=2000");
    }

    [Fact]
    public void AddPagination_AllowsLimitUpTo2000()
    {
        var qb = new QueryBuilder();
        qb.AddPagination(new PaginationParameters { Limit = 2000 });
        qb.Build("/Invoice").ShouldContain("limit=2000");
    }

    [Fact]
    public void AddPagination_ClampsLimitBelowMinTo1()
    {
        var qb = new QueryBuilder();
        qb.AddPagination(new PaginationParameters { Limit = 0 });
        qb.Build("/Invoice").ShouldContain("limit=1");
    }

    [Fact]
    public void Build_EscapesSpecialCharacters()
    {
        var qb = new QueryBuilder();
        qb.Add("name", "Test & Co.");
        qb.Build("/Contact").ShouldContain("name=Test%20%26%20Co.");
    }
}
