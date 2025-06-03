using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
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

        public async Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm)
        {
            const string baseUrl = "summary/projects";

            var queryParams = new Dictionary<string, string?>
            {
                { "email", userEmail }
            };

            if (includeConversions)
            {
                queryParams.Add("includeConversions", "true");
            }

            if (includeTransfers)
            {
                queryParams.Add("includeTransfers", "true");
            }

            if (includeFormAMat)
            {
                queryParams.Add("includeFormAMat", "true");
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryParams.Add("searchTerm", searchTerm);
            }


            var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

            var result = await Get<IEnumerable<AcademisationSummary>>(url);

            return result;
        }
    }

}
