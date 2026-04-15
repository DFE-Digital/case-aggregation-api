using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.SigChange;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromSigChangeSummary(IGetGuidanceLinks getGuidanceLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<SigChangeSummary>
    {
        private const string System = "Significant Change Tracker";

        public UserCaseInfo GetCaseInfo(SigChangeSummary summary)
        {
            return new UserCaseInfo(summary.AcademyName ?? "",
                getSystemLinks.GetSigChangeTitleLink(summary.SigChangeId),
                System,
                summary.ChangeType ?? "Unknown Type",
                summary.CreatedDate,
                summary.UpdatedDate,
                [
                    new CaseInfoItem("Trust", summary.Trust, null),
                    new CaseInfoItem("URN", summary.Urn, null),
                    new CaseInfoItem("Local Authority", summary.LocalAuthority, null),
                    new CaseInfoItem("Region", summary.Region, null),
                    new CaseInfoItem("Date Of Decision", summary.DateOfDecision?.ToString("dd/MM/yyyy"), null)
                ],
                getGuidanceLinks.GenerateLinkItems("SignificantChange"),
                []);
        }
    }
}
