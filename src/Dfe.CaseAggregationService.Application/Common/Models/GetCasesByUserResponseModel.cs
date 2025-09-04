namespace Dfe.CaseAggregationService.Application.Common.Models
{
    public class GetCasesByUserResponseModel
    {
        public int TotalRecordCount { get; set; }

        public List<UserCaseInfo>? CaseInfos { get; set; }
    }
}
