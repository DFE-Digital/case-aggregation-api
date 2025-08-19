using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                CaseType: "CompleteTransfer",
                AcademyName: "Academy Name",
                Urn: "123456",
                ProposedTransferDate: new DateTime(2010, 9, 8),
                IncomingTrust: "Incoming Trust",
                OutgoingTrust: "Outgoing Trust",
                LocalAuthority: "Local Authority",
                CreatedDate: new DateTime(2010, 9, 8),
                UpdatedDate: new DateTime(2010, 9, 9)
            );
            
            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetPrepareTransferTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var underTest = new GetCaseInfoFromCompleteSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);

            var result = underTest.GetCaseInfo(completeSummary);
        }
    }
}
