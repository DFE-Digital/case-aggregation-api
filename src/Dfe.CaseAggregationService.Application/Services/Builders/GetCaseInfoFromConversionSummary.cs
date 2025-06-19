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
            var caseSummaryInfo = summary.TransfersSummary != null ? GetTransferInfo(summary): GetConversionInfo(summary);
            var guidance = GenerateGuidanceLinkItems(summary);
            var resources = GenerateResourcesLinkItems(summary);
            return new UserCaseInfo(GetTitle(summary), getSystemLinks.GetPrepareTitleLink(summary.Id.ToString()), System, GetProjectType(summary), summary.CreatedOn ?? DateTime.MinValue, summary.LastModifiedOn ?? DateTime.MinValue, caseSummaryInfo, guidance, resources);
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

            return [];
        }

        private static string GetTitle(AcademisationSummary summary)
        {
            if (summary.TransfersSummary != null)
            {
                return summary.TransfersSummary.IncomingTrustName!;
            }

            return summary.ConversionsSummary.SchoolName!;
        }

        private static string GetProjectType(AcademisationSummary summary)
        {
            if (summary.TransfersSummary != null)
            {
                return "Transfer";
            }
            return "Conversion";
        }

        private static IEnumerable<CaseInfoItem> GetConversionInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            if(summary.ConversionsSummary.ConversionTransferDate != null)
                yield return new CaseInfoItem("Advisory board date", summary.ConversionsSummary.ConversionTransferDate.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Incoming trust", summary.ConversionsSummary.NameOfTrust!, null);
            yield return new CaseInfoItem("Local authority", summary.ConversionsSummary.LocalAuthority!, null);
            yield return new CaseInfoItem("Route", ProcessConversionRoute(summary), null);
        }

        private static IEnumerable<CaseInfoItem> GetTransferInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            yield return new CaseInfoItem("Proposed transfer date", summary.TransfersSummary.TargetDateForTransfer.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Incoming trust", summary.TransfersSummary.IncomingTrustName!, null);
            yield return new CaseInfoItem("Outgoing trust", summary.TransfersSummary.OutgoingTrustName!, null);
            yield return new CaseInfoItem("Route", summary.TransfersSummary.TypeOfTransfer, null);
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
