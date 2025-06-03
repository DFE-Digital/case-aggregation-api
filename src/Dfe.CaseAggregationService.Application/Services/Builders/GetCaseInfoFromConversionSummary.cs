using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public class GetCaseInfoFromAcademisationSummary: IGetCaseInfo<AcademisationSummary>
    {
        private const string System = "Prepare conversions and transfers";
        public UserCaseInfo GetCaseInfo(AcademisationSummary summary)
        {
            var caseSummaryInfo = summary.TransfersSummary != null ? GetTransferInfo(summary): GetConversionInfo(summary);
            
            return new UserCaseInfo(caseSummaryInfo, GetTitle(summary), "/", System, GetProjectType(summary), summary.CreatedOn ?? DateTime.MinValue, summary.LastModifiedOn ?? DateTime.MinValue);
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
            /*

School name
               URN
               Advisory board date
               Incoming trust
               Local authority

             */
        }

        private static IEnumerable<CaseInfoItem> GetTransferInfo(AcademisationSummary summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            
            yield return new CaseInfoItem("Proposed transfer date", summary.TransfersSummary.TargetDateForTransfer.Value.ToString("dd/MM/yyyy"), null);
            yield return new CaseInfoItem("Incoming trust", summary.TransfersSummary.IncomingTrustName!, null);
            yield return new CaseInfoItem("Outgoing trust", summary.TransfersSummary.OutgoingTrustName!, null);

            /*
            
            Case type
               Academy name
               URN
               Proposed transfer date
               Incoming trust
               Outgoing trust
               Reason 
               LA

             */
        }
    }
}
