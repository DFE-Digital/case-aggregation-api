using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders;
using NSubstitute;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Recast;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromRecastSummaryTests
    {
        [Fact]
        public void GivenPresumptionReturnCaseInfo()
        {
            var mfspSummary = new RecastSummary()
            {
                Id = 1,
                CaseType = "",
                TrustName = "",
                Trn = "",
                DateCaseCreated = new DateTime(2025, 02 ,11),
                RiskToTrust = "",
                DirectionOfTravel = "",
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Is<string>("MfspPresumption")).Returns(new List<LinkItem> { new("Guidance Pres", "http://guide.pres.link") });

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Is<string>("MfspPresumption")).Returns(new List<LinkItem> { new("Resource Pres", "http://res.pres.link") });

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetMfspTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromRecastSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(mfspSummary);

            Assert.NotNull(result);
            Assert.Equal("Free school name", result.Title);
            Assert.Equal("http://TitleLink", result.TitleLink);
            Assert.Equal("Manage free schools projects", result.System);
            Assert.Equal("Presumption", result.ProjectType);
            Assert.Equal(new DateTime(2025, 5, 1), result.CreatedDate);
            Assert.Equal(new DateTime(2025, 5, 1), result.UpdatedDate);
            Assert.Equal(5, result.Info.Count());
            Assert.Equal(result.Guidance.FirstOrDefault().Title, "Guidance Pres");
            Assert.Equal(result.Guidance.FirstOrDefault().Link, "http://guide.pres.link");
            Assert.Equal(result.Resources.FirstOrDefault().Title, "Resource Pres");
            Assert.Equal(result.Resources.FirstOrDefault().Link, "http://res.pres.link");
            var info = result.Info.ToArray();

            info[0].ShouldBe("Trust name", "Magic Trust");
            info[1].ShouldBe("Realistic year of opening", "2024/25");
            info[2].ShouldBe("School type", "School people");
            info[3].ShouldBe("Local authority", "LA Name");
            info[4].ShouldBe("Region", "Wales");
        }

    }
}
