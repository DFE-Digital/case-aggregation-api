using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromMfspSummaryTests
    {
        [Fact]
        public void GivenPresumptionReturnCaseInfo()
        {
            var mfspSummary = new MfspSummary
            {
                ProjectType = "Presumption",
                CurrentName = "Free school name",
                TrustName = "Magic Trust",
                RealisticYearOfOpening = "2024/25",
                SchoolType = "School people",
                LocalAuthority = "LA Name",
                Region = "Wales",
                ProjectStatus = "Pre-opening",
                ProjectManagedBy = "Angela Person",
                UpdatedAt = new DateTime(2025, 5, 1),
                ProjectManagedByEmail = "Angela.Person@education.gov.uk",
                ProjectId = "T12345",
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Is<string>("MfspPresumption")).Returns(new List<LinkItem> { new("Guidance Pres", "http://guide.pres.link")});

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Is<string>("MfspPresumption")).Returns(new List<LinkItem> { new("Resource Pres", "http://res.pres.link") });

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetMfspTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromMfspSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

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


        [Fact]
        public void GivenCentralReturnCaseInfo()
        {
            var mfspSummary = new MfspSummary
            {
                ProjectType = "Central Route",
                CurrentName = "Different Free school name",
                TrustName = "Magic Trust",
                RealisticYearOfOpening = "2024/25",
                SchoolType = "School folks",
                LocalAuthority = "LA Name",
                Region = "Wales",
                ProjectStatus = "Pre-opening",
                ProjectManagedBy = "Andy Person",
                UpdatedAt = new DateTime(2025, 5, 2),
                ProjectManagedByEmail = "Andy.Person@education.gov.uk",
                ProjectId = "T12345",
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Is<string>("MfspCentral")).Returns(new List<LinkItem> { new("Guidance Pres", "http://guide.pres.link") });

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Is<string>("MfspCentral")).Returns(new List<LinkItem> { new("Resource Pres", "http://res.pres.link") });

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetMfspTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromMfspSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(mfspSummary);

            Assert.NotNull(result);
            Assert.Equal("Different Free school name", result.Title);
            Assert.Equal("http://TitleLink", result.TitleLink);
            Assert.Equal("Manage free schools projects", result.System);
            Assert.Equal("Central Route", result.ProjectType);
            Assert.Equal(new DateTime(2025, 5, 2), result.CreatedDate);
            Assert.Equal(new DateTime(2025, 5, 2), result.UpdatedDate);
            Assert.Equal(5, result.Info.Count());
            Assert.Equal(result.Guidance.FirstOrDefault().Title, "Guidance Pres");
            Assert.Equal(result.Guidance.FirstOrDefault().Link, "http://guide.pres.link");
            Assert.Equal(result.Resources.FirstOrDefault().Title, "Resource Pres");
            Assert.Equal(result.Resources.FirstOrDefault().Link, "http://res.pres.link");
            var info = result.Info.ToArray();

            info[0].ShouldBe("Trust name", "Magic Trust");
            info[1].ShouldBe("Realistic year of opening", "2024/25");
            info[2].ShouldBe("School type", "School folks");
            info[3].ShouldBe("Local authority", "LA Name");
            info[4].ShouldBe("Region", "Wales");
        }
    }
}
