namespace Dfe.CaseAggregationService.Infrastructure.Gateways;
public class ApiPagingInfo
{
   public int Page { get; set; }
   public int RecordCount { get; set; }
   public string? NextPageUrl { get; set; }
}
