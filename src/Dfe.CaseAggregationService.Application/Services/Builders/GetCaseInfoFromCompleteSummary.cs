using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Complete;
using GovUK.Dfe.CoreLibs.Utilities.Extensions;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromCompleteSummary(IGetGuidanceLinks getGuidanceLinks, IGetResourcesLinks getResourcesLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<CompleteSummary>
    {
        private const string System = "Complete conversions, transfers and changes";
        public UserCaseInfo GetCaseInfo(CompleteSummary summary)
        {
            return new UserCaseInfo(GetTitle(summary),
                getSystemLinks.GetCompleteTitleLink(summary.ProjectId.Value.ToString()),
                System,
                summary.CaseType.ToDescription(),
                summary.CreatedDate ?? DateTime.MinValue,
                summary.UpdatedDate ?? DateTime.MinValue,
                GetCaseInfoItems(summary),
                GenerateGuidanceLinkItems(summary),
                GenerateResourceLinkItems(summary));
        }

        private IEnumerable<LinkItem> GenerateGuidanceLinkItems(CompleteSummary summary)
        {
            if (summary.CaseType is CompleteProjectType.Transfer or CompleteProjectType.FormAMatTransfer)
                return getGuidanceLinks.GenerateLinkItems("CompleteTransfer");
            if (summary.CaseType is CompleteProjectType.Conversion or CompleteProjectType.FormAMatConversion)
                return getGuidanceLinks.GenerateLinkItems("CompleteConversion");
            return [];
        }

        private IEnumerable<LinkItem> GenerateResourceLinkItems(CompleteSummary summary)
        {

            if (summary.CaseType is CompleteProjectType.Transfer or CompleteProjectType.FormAMatTransfer)
                return getResourcesLinks.GenerateLinkItems("CompleteTransfer");
            if (summary.CaseType is CompleteProjectType.Conversion or CompleteProjectType.FormAMatConversion)
                return getResourcesLinks.GenerateLinkItems("CompleteConversion");
            return [];
        }

        private string GetTitle(CompleteSummary summary)
        {
            return summary.AcademyName;
        }

        private static IEnumerable<CaseInfoItem> GetCaseInfoItems(CompleteSummary summary)
        {
            if (summary.CaseType is CompleteProjectType.Conversion or CompleteProjectType.FormAMatConversion)
            {
                yield return new CaseInfoItem("Current conversion date",
                    summary.ProposedTransferDate.HasValue
                        ? summary.ProposedTransferDate.Value.ToString("dd/MM/yyyy")
                        : "", null);
                yield return new CaseInfoItem("Name", summary.IncomingTrust, null);
                yield return new CaseInfoItem("LA", summary.LocalAuthority, null);
            }

            if (summary.CaseType is CompleteProjectType.Transfer or CompleteProjectType.FormAMatTransfer)
            {
                yield return new CaseInfoItem("Current transfer date",
                    summary.ProposedTransferDate.HasValue
                        ? summary.ProposedTransferDate.Value.ToString("dd/MM/yyyy")
                        : "", null);
                yield return new CaseInfoItem("Incoming trust", summary.IncomingTrust, null);
                yield return new CaseInfoItem("Outgoing trust", summary.OutgoingTrust, null);
                yield return new CaseInfoItem("LA", summary.LocalAuthority, null);
            }
        }
    }
}
