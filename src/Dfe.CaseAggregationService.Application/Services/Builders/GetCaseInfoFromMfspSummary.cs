using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromMfspSummary(IGetGuidanceLinks getGuidanceLinks, IGetResourcesLinks getResourcesLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<MfspSummary>
    {
        private const string System = "Manage free schools projects";
        public UserCaseInfo GetCaseInfo(MfspSummary summary)
        {
            return new UserCaseInfo(GetTitle(summary),
                getSystemLinks.GetMfspTitleLink(summary.ProjectId),
                System,
                summary.ProjectType,
                summary.UpdatedAt,
                summary.UpdatedAt,
                GetCaseInfoItems(summary),
                GenerateGuidanceLinkItems(summary),
                GenerateResourceLinkItems(summary));
        }

        private IEnumerable<LinkItem> GenerateGuidanceLinkItems(MfspSummary summary)
        {

            return summary.ProjectType switch
            {
                "Presumption" => getGuidanceLinks.GenerateLinkItems("MfspPresumption"),
                "Central Route" => getGuidanceLinks.GenerateLinkItems("MfspCentral"),
                _ => []
            };
        }
        private IEnumerable<LinkItem> GenerateResourceLinkItems(MfspSummary summary)
        {

            return summary.ProjectType switch
            {
                "Presumption" => getResourcesLinks.GenerateLinkItems("MfspPresumption"),
                "Central Route" => getResourcesLinks.GenerateLinkItems("MfspCentral"),
                _ => []
            };
        }

        private string GetTitle(MfspSummary summary)
        {
            return summary.CurrentName;
        }

        private static IEnumerable<CaseInfoItem> GetCaseInfoItems(MfspSummary summary)
        {
            yield return new CaseInfoItem("Trust name", summary.TrustName , null);
            yield return new CaseInfoItem("Realistic year of opening", summary.RealisticYearOfOpening , null);
            yield return new CaseInfoItem("School type", summary.SchoolType , null);
            yield return new CaseInfoItem("Local authority", summary.LocalAuthority , null);
            yield return new CaseInfoItem("Region", summary.Region , null);

        }
        
    }
}
