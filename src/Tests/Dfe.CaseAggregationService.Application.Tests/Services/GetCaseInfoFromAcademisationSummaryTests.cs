using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.Services
{
    public class GetCaseInfoFromAcademisationSummaryTests
    {
        [Fact]
        public void GivenTransferReturnCaseInfo()
        {
            var academySummary = new AcademisationSummary();

            academySummary.TransfersSummary = new TransfersSummary()
            {
                IncomingTrustName = "Academy Name",
                OutgoingTrustName = "Outgoing Trust Name",
                TargetDateForTransfer = new DateTime(2010, 9, 8),
                TypeOfTransfer = "Transfer Type",
                Urn = 123456,
            };

            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetPrepareTransferTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromAcademisationSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var caseInfo = underTest.GetCaseInfo(academySummary);
            
            Assert.NotNull(caseInfo);
            
            Assert.Equal("Transfer", caseInfo.ProjectType);
            Assert.Equal("Prepare conversions and transfers", caseInfo.System);
            Assert.Equal("http://TitleLink", caseInfo.TitleLink);
            Assert.Equal("Academy Name", caseInfo.Title);

            Assert.Equal(5, caseInfo.Info.Count());
            var info = caseInfo.Info.ToArray();

            Assert.Equal("URN", info[0].Label);
            Assert.Equal("123456", info[0].Value);
            Assert.Null(info[0].Link);
            Assert.Equal("Proposed transfer date", info[1].Label);
            Assert.Equal("08/09/2010", info[1].Value);
            Assert.Null(info[1].Link);
            Assert.Equal("Incoming trust", info[2].Label);
            Assert.Equal("Academy Name", info[2].Value);
            Assert.Null(info[2].Link);
            Assert.Equal("Outgoing trust", info[3].Label);
            Assert.Equal("Outgoing Trust Name", info[3].Value);
            Assert.Null(info[3].Link);
            Assert.Equal("Route", info[4].Label);
            Assert.Equal("Transfer Type", info[4].Value);
            Assert.Null(info[4].Link);

        }

        [Fact]
        public void GivenConversionReturnCaseInfo()
        {
            var academySummary = new AcademisationSummary();

            academySummary.ConversionsSummary = new ConversionsSummary()
            {
                SchoolName = "Academy Name",
                LocalAuthority = "LA Name",
                NameOfTrust = "Trust Name",
                ConversionTransferDate = new DateTime(2010, 9, 8),
                AcademyTypeAndRoute = "Converter",
                Urn = 123456,
            };
            
            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetPrepareConversionTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromAcademisationSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);
            
            var caseInfo = underTest.GetCaseInfo(academySummary);

            Assert.NotNull(caseInfo);

            Assert.Equal("Conversion", caseInfo.ProjectType);
            Assert.Equal("Prepare conversions and transfers", caseInfo.System);
            Assert.Equal("http://TitleLink", caseInfo.TitleLink);
            Assert.Equal("Academy Name", caseInfo.Title);

            Assert.Equal(5, caseInfo.Info.Count());
            var info = caseInfo.Info.ToArray();

            Assert.Equal("URN", info[0].Label);
            Assert.Equal("123456", info[0].Value);
            Assert.Null(info[0].Link);
            Assert.Equal("Advisory board date", info[1].Label);
            Assert.Equal("08/09/2010", info[1].Value);
            Assert.Null(info[1].Link);
            Assert.Equal("Incoming trust", info[2].Label);
            Assert.Equal("Trust Name", info[2].Value);
            Assert.Null(info[2].Link);
            Assert.Equal("Local authority", info[3].Label);
            Assert.Equal("LA Name", info[3].Value);
            Assert.Null(info[3].Link);
            Assert.Equal("Route", info[4].Label);
            Assert.Equal("Voluntary conversion", info[4].Value);
            Assert.Null(info[4].Link);

        }
    }
}
