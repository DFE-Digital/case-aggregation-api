using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class AcademisationApiClient : ApiClient, IGetAcademisationSummary
    {
        public AcademisationApiClient(
            IHttpClientFactory clientFactory, 
            ILogger<ApiClient> logger,
            string httpClientName = "AcademisationApiClient") : base(clientFactory, logger, httpClientName)
        {
            
        }

        public async Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail)
        {
            const string baseUrl = "summary/projects";

            var url = $"{baseUrl}?email={userEmail}";

            var result = await Get<IEnumerable<AcademisationSummary>>(url);

            return result;
        }
    }

}
