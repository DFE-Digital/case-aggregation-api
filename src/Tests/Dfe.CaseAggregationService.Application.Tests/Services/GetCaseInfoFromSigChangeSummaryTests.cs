using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using NSubstitute;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.SigChange;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromSigChangeSummaryTests
    {
        [Fact]
        public void GivenPresumptionReturnCaseInfo()
        {
            var sigChangeSummary = new SigChangeSummary()
            {
                SigChangeId = "1",
                AcademyName = "Academy 1",
                ChangeType = "Type 1",
                Trust = "Trust 1",
                Urn = "123456",
                LocalAuthority = "LA 1",
                Region = "Region 1",
                DateOfDecision = new DateTime(2025, 5, 1),
                CreatedDate = new DateTime(2025, 5, 1),
                UpdatedDate = new DateTime(2025, 5, 1)
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Is<string>("SignificantChange")).Returns(new List<LinkItem> { new("Guidance Pres", "http://guide.pres.link") });

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetSigChangeTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromSigChangeSummary(getGuidanceLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(sigChangeSummary);

            Assert.NotNull(result);
            
            Assert.Equal(new DateTime(2025, 5, 1), result.CreatedDate);
            Assert.Equal(new DateTime(2025, 5, 1), result.UpdatedDate);
            Assert.Equal("Type 1", result.ProjectType);
            Assert.Equal(5, result.Info.Count());
            Assert.Equal(result.Guidance.FirstOrDefault().Title, "Guidance Pres");
            Assert.Equal(result.Guidance.FirstOrDefault().Link, "http://guide.pres.link");
            var info = result.Info.ToArray();

            info[0].ShouldBe("Trust", "Trust 1");
            info[1].ShouldBe("URN", "123456");
            info[2].ShouldBe("Local Authority", "LA 1");
            info[3].ShouldBe("Region", "Region 1");
            info[4].ShouldBe("Date Of Decision", "01/05/2025");


        }

    }
}
