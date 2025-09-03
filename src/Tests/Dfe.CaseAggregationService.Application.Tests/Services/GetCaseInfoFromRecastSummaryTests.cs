using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders;
using NSubstitute;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Recast;
using FluentAssertions;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromRecastSummaryTests
    {
        [Fact]
        public void GivenNonComplilanceReturnCaseInfo()
        {
            var mfspSummary = new RecastSummary()
            {
                Id = 1,
                CaseType = "Non-compliance",
                TrustName = "Test Trust",
                Trn = "TR00001",
                DateCaseCreated = new DateTime(2025, 02 ,11),
                RiskToTrust = "None",
                DirectionOfTravel = "Forward",
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Is<string>("RecastNonCompliance")).Returns(new List<LinkItem> { new("Guidance Pres", "http://guide.pres.link") });

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Is<string>("RecastNonCompliance")).Returns(new List<LinkItem> { new("Resource Pres", "http://res.pres.link") });

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetRecastTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromRecastSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(mfspSummary);

            Assert.NotNull(result);
            Assert.Equal("Test Trust", result.Title);
            Assert.Equal("http://TitleLink", result.TitleLink);
            Assert.Equal("Record concerns and support for trusts", result.System);
            Assert.Equal("Non-compliance", result.ProjectType);
            Assert.Equal(new DateTime(2025, 02, 11), result.CreatedDate);
            Assert.Equal(new DateTime(2025, 02, 11), result.UpdatedDate);
            Assert.Equal(5, result.Info.Count());
            Assert.Equal(result.Guidance.FirstOrDefault().Title, "Guidance Pres");
            Assert.Equal(result.Guidance.FirstOrDefault().Link, "http://guide.pres.link");
            Assert.Equal(result.Resources.FirstOrDefault().Title, "Resource Pres");
            Assert.Equal(result.Resources.FirstOrDefault().Link, "http://res.pres.link");
            var info = result.Info.ToArray();

            info[0].ShouldBe("Concerns", "None");
            info[1].ShouldBe("Trust", "Test Trust");
            info[2].ShouldBe("Group ID", "TR00001");
            info[3].ShouldBe("Date created", "11/02/2025");
            info[4].ShouldBe("Risk to trust", "None");
        }

    }
}
