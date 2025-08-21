using Dfe.AcademiesApi.Client.Contracts;
using Dfe.CaseAggregationService.Domain.Entities.Recast;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class RecastApiClient : ApiClient, IRecastRepository
    {
        private readonly ITrustsV4Client _trustsClient;

        public RecastApiClient(
            IHttpClientFactory clientFactory,
            ILogger<ApiClient> logger,
            ITrustsV4Client trustsClient,
            string httpClientName = "RecastApiClient") : base(clientFactory, logger, httpClientName)
        {
            _trustsClient = trustsClient;
        }

        public async Task<IEnumerable<RecastSummary>> GetRecastSummaries(string userEmail,
            string[]? requestFilterProjectTypes)
        {
            var baseUrl = $"/v2/concerns-cases/summary/{userEmail}/active";

            var queryParams = new Dictionary<string, string?>
            {
                { "page", "1" },
                { "count", "100" }
            };

            var url = QueryHelpers.AddQueryString(baseUrl, queryParams);
            
            var headers = new Dictionary<string, string>
            {
                { "x-user-context-name", userEmail },
                { "x-user-context-role-0", "concerns-casework.caseworker" }
            };

            var result = await Get<ApiResponseV2<ActiveCaseSummaryResponse>>(url, headers);

            if (!result.Data.Any())
                return [];

            var trusts = await _trustsClient.GetByUkprnsAllAsync(result.Data.Select(x => x.TrustUkPrn));

            var output = result.Data.Where(x => x.ActiveConcerns.Any()).Select(x => new RecastSummary
            {
                Id = x.CaseUrn,
                CaseType = GetCaseType(x),
                TrustName = trusts.FirstOrDefault(t => t.Ukprn == x.TrustUkPrn)?.Name ?? "",
                Trn = trusts.FirstOrDefault(t => t.Ukprn == x.TrustUkPrn)?.ReferenceNumber ?? "",
                DateCaseCreated = x.CreatedAt,
                RiskToTrust = x.Rating.Name
            });

            if (requestFilterProjectTypes is { Length: > 0 })
            {
                return output.Where(x => requestFilterProjectTypes.Contains(x.CaseType));
            }

            return output;
        }
        
        private string GetCaseType(ActiveCaseSummaryResponse summary)
        {
            return summary.ActiveConcerns.FirstOrDefault()?.Name ?? "";
        }

    }
}
