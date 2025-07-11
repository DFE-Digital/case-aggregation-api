using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromAcademisationSummary(IGetGuidanceLinks getGuidanceLinks, IGetResourcesLinks getResourcesLinks, IGetSystemLinks getSystemLinks) : IGetCaseInfo<AcademisationSummary>
    {
        private const string System = "Prepare conversions and transfers";
        public UserCaseInfo GetCaseInfo(AcademisationSummary summary)
        {
            var caseSummaryInfo = GetCaseSummaries(summary);
            var guidance = GenerateGuidanceLinkItems(summary);
            var resources = GenerateResourcesLinkItems(summary);
            return new UserCaseInfo(GetTitle(summary), getSystemLinks.GetPrepareTitleLink(summary.Id.ToString()), System, GetProjectType(summary), summary.CreatedOn ?? DateTime.MinValue, summary.LastModifiedOn ?? DateTime.MinValue, caseSummaryInfo, guidance, resources);
        }

        private static IEnumerable<CaseInfoItem> GetCaseSummaries(AcademisationSummary summary)
        {
            if (summary.ConversionsSummary != null)
                return GetConversionInfo(summary);

            if (summary.FormAMatSummary != null)
                return GetFormAMatInfo(summary);

            if (summary.TransfersSummary != null)
                return GetTransferInfo(summary);

            return [];
        }

        private IEnumerable<LinkItem> GenerateGuidanceLinkItems(AcademisationSummary summary)
        {
            if (summary.ConversionsSummary != null)
            {
                return summary.ConversionsSummary?.AcademyTypeAndRoute switch
                {
                    "Converter" => getGuidanceLinks.GenerateLinkItems("PrepareVoluntaryConversion"),
                    "Sponsored" => getGuidanceLinks.GenerateLinkItems("PrepareSponsoredConversion"),
                    _ => []
                };
            }

            if (summary.TransfersSummary != null)
            {
                return getGuidanceLinks.GenerateLinkItems("PrepareTransfer");
            }

            if (summary.FormAMatSummary != null)
            {
                return getGuidanceLinks.GenerateLinkItems("PrepareFormAMat");
            }

            return [];
        }

        private IEnumerable<LinkItem> GenerateResourcesLinkItems(AcademisationSummary summary)
        {
            if (summary.ConversionsSummary != null)
            {
                return summary.ConversionsSummary?.AcademyTypeAndRoute switch
                {
                    "Converter" => getResourcesLinks.GenerateLinkItems("PrepareVoluntaryConversion"),
                    "Sponsored" => getResourcesLinks.GenerateLinkItems("PrepareSponsoredConversion"),
                    _ => []
                };
            }

            if (summary.TransfersSummary != null)
            {
                return getResourcesLinks.GenerateLinkItems("PrepareTransfer");
            }

            if (summary.FormAMatSummary != null)
            {
                return getResourcesLinks.GenerateLinkItems("PrepareFormAMat");
            }

            return [];
        }

        private static string GetTitle(AcademisationSummary summary)
        {
            if (summary.TransfersSummary != null)
            {
                return summary.TransfersSummary.IncomingTrustName;
            }

            if (summary.ConversionsSummary != null)
            {
                return summary.ConversionsSummary.SchoolName!;
            }

            if (summary.FormAMatSummary != null)
            {
                return summary.FormAMatSummary.ProposedTrustName!;
            }

            return "";
        }

        private static string GetProjectType(AcademisationSummary summary)
        {
            if (summary.TransfersSummary != null)
            {
                return "Transfer";
            }

            if (summary.ConversionsSummary != null)
            {
                return "Conversion";
            }

            if (summary.FormAMatSummary != null)
            {
                return "Form a MAT";
            }

            return "";
        }

        private static IEnumerable<CaseInfoItem> GetConversionInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.ConversionsSummary.Urn.ToString(), null);
            if(summary.ConversionsSummary.ConversionTransferDate != null)
                yield return new CaseInfoItem("Advisory board date", summary.ConversionsSummary.ConversionTransferDate.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Incoming trust", summary.ConversionsSummary.NameOfTrust!, null);
            yield return new CaseInfoItem("Local authority", summary.ConversionsSummary.LocalAuthority!, null);
            yield return new CaseInfoItem("Route", ProcessConversionRoute(summary), null);
        }

        private static IEnumerable<CaseInfoItem> GetTransferInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.TransfersSummary.Urn.ToString(), null);
            if(summary.TransfersSummary.TargetDateForTransfer != null)
                yield return new CaseInfoItem("Proposed transfer date", summary.TransfersSummary.TargetDateForTransfer.Value.ToString("dd/MM/yyyy"), null);
            
            yield return new CaseInfoItem("Incoming trust", summary.TransfersSummary.IncomingTrustName, null);
            yield return new CaseInfoItem("Outgoing trust", summary.TransfersSummary.OutgoingTrustName!, null);
            yield return new CaseInfoItem("Route", summary.TransfersSummary.TypeOfTransfer, null);
        }

        private static IEnumerable<CaseInfoItem> GetFormAMatInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("School names", summary.FormAMatSummary.SchoolNames.Aggregate((acc, next) => acc + ", " + next), null);
            if (summary.FormAMatSummary.AdvisoryBoardDate != null)
                yield return new CaseInfoItem("Advisory board date", summary.FormAMatSummary.AdvisoryBoardDate.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Local authority(s) involved", summary.FormAMatSummary.LocalAuthority.Aggregate((acc, next) => acc + ", " + next), null);
        }

        private static string? ProcessConversionRoute(AcademisationSummary input)
        {
            return input.ConversionsSummary?.AcademyTypeAndRoute switch
            {
                "Converter" => "Voluntary conversion",
                "Sponsored" => "Sponsored conversion",
                _ => null
            };

        }
    }
}
