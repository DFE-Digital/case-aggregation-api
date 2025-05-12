namespace Dfe.CaseAggregationService.Infrastructure.Gateways;

public sealed class ApiWrapper<T>
{
   public T Data { get; set; }
   public ApiPagingInfo Paging { get; set; }
}
