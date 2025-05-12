using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class CompleteApiClient : ApiClient
    {
        public CompleteApiClient(
            IHttpClientFactory clientFactory, 
            ILogger<ApiClient> logger,
            string httpClientName = "CompleteApiClient") : base(clientFactory, logger, httpClientName)
        {
            
        }
    }
}
