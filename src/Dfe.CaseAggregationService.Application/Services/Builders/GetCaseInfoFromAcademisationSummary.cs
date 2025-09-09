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
            return new UserCaseInfo(GetTitle(summary),
                GetTitleLink(summary),
                System,
                GetProjectType(summary),
                summary.CreatedOn ?? DateTime.MinValue,
                summary.LastModifiedOn ?? DateTime.MinValue,
                caseSummaryInfo,
                guidance,
                resources);
        }

        private string GetTitleLink(AcademisationSummary summary)
        {
            if (summary.ConversionsSummary != null)
            {
                return getSystemLinks.GetPrepareConversionTitleLink(summary.Id.ToString());
            }

            if (summary.TransfersSummary != null)
            {
                return getSystemLinks.GetPrepareTransferTitleLink(summary.Id.ToString());
            }

            if (summary.FormAMatSummary != null)
            {
                return getSystemLinks.GetPrepareFormAMatTitleLink(summary.Id.ToString());
            }

            return string.Empty;
        }

        private static IEnumerable<CaseInfoItem> GetCaseSummaries(AcademisationSummary summary)
        {
            if (summary.ConversionsSummary != null)
                return GetConversionInfo(summary.ConversionsSummary);

            if (summary.FormAMatSummary != null)
                return GetFormAMatInfo(summary.FormAMatSummary);

            if (summary.TransfersSummary != null)
                return GetTransferInfo(summary.TransfersSummary);

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

            return string.Empty;
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

            return string.Empty;
        }

        private static IEnumerable<CaseInfoItem> GetConversionInfo(ConversionsSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            if(summary.ConversionTransferDate != null)
                yield return new CaseInfoItem("Advisory board date", summary.ConversionTransferDate.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Incoming trust", summary.NameOfTrust!, null);
            yield return new CaseInfoItem("Local authority", summary.LocalAuthority!, null);
            yield return new CaseInfoItem("Route", ProcessConversionRoute(summary), null);
        }

        private static IEnumerable<CaseInfoItem> GetTransferInfo(TransfersSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            if(summary.TargetDateForTransfer != null)
                yield return new CaseInfoItem("Proposed transfer date", summary.TargetDateForTransfer.Value.ToString("dd/MM/yyyy"), null);
            
            yield return new CaseInfoItem("Incoming trust", summary.IncomingTrustName, null);
            yield return new CaseInfoItem("Outgoing trust", summary.OutgoingTrustName!, null);
            yield return new CaseInfoItem("Route", summary.TypeOfTransfer, null);
        }

        private static IEnumerable<CaseInfoItem> GetFormAMatInfo(FormAMatSummary summary)
        {
            if (summary == null)
                throw new ArgumentNullException();

            yield return new CaseInfoItem("School names", summary.SchoolNames.Aggregate((acc, next) => acc + ", " + next), null);
            if (summary.AdvisoryBoardDate != null)
                yield return new CaseInfoItem("Advisory board date", summary.AdvisoryBoardDate.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Local authority(s) involved", summary.LocalAuthority.Aggregate((acc, next) => acc + ", " + next), null);
        }

        private static string? ProcessConversionRoute(ConversionsSummary input)
        {
            return input.AcademyTypeAndRoute switch
            {
                "Converter" => "Voluntary conversion",
                "Sponsored" => "Sponsored conversion",
                _ => null
            };

        }
    }
}
