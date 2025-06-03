using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.SignificantChange.Client.Contracts;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public interface IGetCaseInfo<in T>
    {
        UserCaseInfo GetCaseInfo(T summary);
    }

    public class GetCaseInfoFromSigChange : IGetCaseInfo<SignificantChangeCase>
    {
        private const string System = "Prepare conversions and transfers";
        public UserCaseInfo GetCaseInfo(SignificantChangeCase summary)
        {
            var caseSummaryInfo = GetConversionInfo(summary);

            return new UserCaseInfo(caseSummaryInfo, GetTitle(summary), "/", System, GetProjectType(summary), DateTime.MinValue, DateTime.MinValue);
        }

        private static string GetTitle(SignificantChangeCase summary)
        {
            return summary.Academy ?? "";
        }

        private static string GetProjectType(SignificantChangeCase summary)
        {
            return summary.CaseType ?? "";
        }

        private static IEnumerable<CaseInfoItem> GetConversionInfo(SignificantChangeCase summary)
        {
            yield return new CaseInfoItem("URN", summary.Urn.ToString(), null);
            yield return new CaseInfoItem("Trust", summary.Trust!, null);
            yield return new CaseInfoItem("Local authority", summary.La, null);
            yield return new CaseInfoItem("Region", summary.Region, null);
            yield return new CaseInfoItem("Date of decision", summary.DateOfDecision?.ToString("dd/MM/yyyy"), null);
            /*

Academy
               Trust
               URN
               LA
               Region
               Date of decision

             */
        }

    }
}
