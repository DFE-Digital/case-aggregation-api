using Dfe.CaseAggregationService.Application.Common.Models;

namespace Dfe.CaseAggregationService.Application.Tests.Services;

public static class CaseInfoItemTestExtensions
{
    public static void ShouldBe(this CaseInfoItem item, string expectedLabel, string expectedValue)
    {
        Assert.Equal(expectedLabel, item.Label);
        Assert.Equal(expectedValue, item.Value);
        Assert.Null(item.Link);
    }

    public static void ShouldBe(this CaseInfoItem item, string expectedLabel, string expectedValue, string expectedLink)
    {
        Assert.Equal(expectedLabel, item.Label);
        Assert.Equal(expectedValue, item.Value);
        Assert.Equal(expectedLink, item.Link);
    }
}