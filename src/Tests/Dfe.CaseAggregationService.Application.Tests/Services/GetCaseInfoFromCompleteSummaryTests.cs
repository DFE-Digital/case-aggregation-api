using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders;
using NSubstitute;
using Dfe.CaseAggregationService.Domain.Entities.Complete;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromCompleteSummaryTests
    {
        [Fact]
        public void GivenTransferReturnCaseInfo()
        {
            var completeSummary = new CompleteSummary(
                ProjectId: Guid.NewGuid(),
                CaseType: CompleteProjectType.Transfer,
                AcademyName: "Academy Name",
                Urn: "123456",
                ProposedTransferDate: new DateTime(2010, 9, 8),
                IncomingTrust: "Incoming Trust name",
                OutgoingTrust: "Outgoing Trust name",
                LocalAuthority: "Local Authority",
                CreatedDate: new DateTime(2010, 9, 8),
                UpdatedDate: new DateTime(2010, 9, 9)
            );

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetCompleteTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromCompleteSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(completeSummary);

            Assert.NotNull(result);
            Assert.Equal("Academy Name", result.Title);
            Assert.Equal("http://TitleLink", result.TitleLink);
            Assert.Equal("Complete conversions, transfers and changes", result.System);
            Assert.Equal("Transfer", result.ProjectType);
            Assert.Equal(new DateTime(2010, 9, 8), result.CreatedDate);
            Assert.Equal(new DateTime(2010, 9, 9), result.UpdatedDate);
            Assert.Equal(4, result.Info.Count());
            Assert.Empty(result.Guidance);
            Assert.Empty(result.Resources);
            var info = result.Info.ToArray();

            info[0].ShouldBe("Current transfer date", "08/09/2010");
            info[1].ShouldBe("Incoming trust", "Incoming Trust name");
            info[2].ShouldBe("Outgoing trust", "Outgoing Trust name");
            info[3].ShouldBe("LA", "Local Authority");
        }

        [Fact]
        public void GivenConversionReturnCaseInfo()
        {
            var completeSummary = new CompleteSummary(
                ProjectId: Guid.NewGuid(),
                CaseType: CompleteProjectType.Conversion,
                AcademyName: "Academy Name",
                Urn: "123456",
                ProposedTransferDate: new DateTime(2010, 9, 8),
                IncomingTrust: "Incoming Trust name",
                OutgoingTrust: "Outgoing Trust name",
                LocalAuthority: "Local Authority",
                CreatedDate: new DateTime(2010, 9, 8),
                UpdatedDate: new DateTime(2010, 9, 9)
            );

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetCompleteTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromCompleteSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(completeSummary);

            Assert.NotNull(result);
            Assert.Equal("Academy Name", result.Title);
            Assert.Equal("http://TitleLink", result.TitleLink);
            Assert.Equal("Complete conversions, transfers and changes", result.System);
            Assert.Equal("Conversion", result.ProjectType);
            Assert.Equal(new DateTime(2010, 9, 8), result.CreatedDate);
            Assert.Equal(new DateTime(2010, 9, 9), result.UpdatedDate);
            Assert.Equal(3, result.Info.Count());
            Assert.Empty(result.Guidance);
            Assert.Empty(result.Resources);
            var info = result.Info.ToArray();

            info[0].ShouldBe("Current conversion date", "08/09/2010");
            info[1].ShouldBe("Name", "Incoming Trust name");
            info[2].ShouldBe("LA", "Local Authority");
        }
    }
}
