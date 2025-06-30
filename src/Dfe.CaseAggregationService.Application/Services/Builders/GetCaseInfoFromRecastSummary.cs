using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Recast;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromRecastSummary(IGetGuidanceLinks getGuidanceLinks, IGetResourcesLinks getResourcesLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<RecastSummary>
    {
        private const string System = "Record concerns and support for trusts";
        
        public UserCaseInfo GetCaseInfo(RecastSummary summary)
        {
            GenerateGuidanceLinkItems(summary);
            getResourcesLinks.GenerateLinkItems("Recast");
            return new UserCaseInfo(GetTitle(summary),
                getSystemLinks.GetRecastTitleLink(summary.Trn),
                System,
                summary.CaseType,
                summary.DateCaseCreated,
                DateTime.Now,
                GetCaseInfoItems(summary),
                GenerateGuidanceLinkItems(summary),
                GenerateResourceLinkItems(summary));
        }

        private IEnumerable<LinkItem> GenerateGuidanceLinkItems(RecastSummary summary)
        {

            return summary.CaseType switch
            {
                "Safeguarding non-compliance" => getGuidanceLinks.GenerateLinkItems("RecastSafeguarding"),
                "Non-compliance" => getGuidanceLinks.GenerateLinkItems("RecastNonCompliance"),
                "Governance capability" => getGuidanceLinks.GenerateLinkItems("RecastGovernanceCapability"),
                _ => []
            };
        }
        private IEnumerable<LinkItem> GenerateResourceLinkItems(RecastSummary summary)
        {

            return summary.CaseType switch
            {
                "Safeguarding non-compliance" => getResourcesLinks.GenerateLinkItems("RecastSafeguarding"),
                "Non-compliance" => getResourcesLinks.GenerateLinkItems("RecastNonCompliance"),
                "Governance capability" => getResourcesLinks.GenerateLinkItems("RecastGovernanceCapability"),
                _ => []
            };
        }

        private string GetTitle(RecastSummary summary)
        {
            return summary.TrustName;
        }

        private static IEnumerable<CaseInfoItem> GetCaseInfoItems(RecastSummary summary)
        {
            yield return new CaseInfoItem("Concerns", summary.RiskToTrust, null);
            yield return new CaseInfoItem("Trust", summary.TrustName, null);
            yield return new CaseInfoItem("Group ID", summary.Trn, null);
            yield return new CaseInfoItem("Date created", summary.DateCaseCreated.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Risk to trust", summary.RiskToTrust, null);
        }
    }
}
