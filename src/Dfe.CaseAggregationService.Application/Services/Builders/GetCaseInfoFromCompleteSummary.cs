using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Complete;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromCompleteSummary(IGetGuidanceLinks getGuidanceLinks, IGetResourcesLinks getResourcesLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<CompleteSummary>
    {
        private const string System = "Complete";
        public UserCaseInfo GetCaseInfo(CompleteSummary summary)
        {
            GenerateGuidanceLinkItems(summary);
            getResourcesLinks.GenerateLinkItems("Recast");
            return new UserCaseInfo(GetTitle(summary),
                getSystemLinks.GetMfspTitleLink(summary.ProjectId),
                System,
                summary.CaseType,
                summary.CreatedDate,
                summary.UpdatedDate,
                GetCaseInfoItems(summary),
                GenerateGuidanceLinkItems(summary),
                GenerateResourceLinkItems(summary));
        }

        private IEnumerable<LinkItem> GenerateGuidanceLinkItems(CompleteSummary summary)
        {

            return summary.CaseType switch
            {
                "Presumption" => getGuidanceLinks.GenerateLinkItems("MfspPresumption"),
                "Central Route" => getGuidanceLinks.GenerateLinkItems("MfspCentral"),
                _ => []
            };
        }
        private IEnumerable<LinkItem> GenerateResourceLinkItems(CompleteSummary summary)
        {

            return summary.ProjectType switch
            {
                "Presumption" => getGuidanceLinks.GenerateLinkItems("MfspPresumption"),
                "Central Route" => getGuidanceLinks.GenerateLinkItems("MfspCentral"),
                _ => []
            };
        }

        private string GetTitle(MfspSummary summary)
        {
            return summary.CurrentName;
        }

        private static IEnumerable<CaseInfoItem> GetCaseInfoItems(CompleteSummary summary)
        {
            yield return new CaseInfoItem("Trust name", summary.TrustName, null);
            yield return new CaseInfoItem("Realistic year of opening", summary.RealisticYearOfOpening, null);
            yield return new CaseInfoItem("School type", summary.SchoolType, null);
            yield return new CaseInfoItem("Local authority", summary.LocalAuthority, null);
            yield return new CaseInfoItem("Region", summary.Region, null);

        }
    }
}
