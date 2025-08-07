
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.CaseAggregationService.Infrastructure.Dto;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class MfspApiClient : ApiClient, IMfspRepository
    {
        public MfspApiClient(
            IHttpClientFactory clientFactory,
            ILogger<ApiClient> logger,
            string httpClientName = "MfspApiClient") : base(clientFactory, logger, httpClientName)
        {
        }

        public async Task<IEnumerable<MfspSummary>> GetMfspSummaries(string userEmail,
            string[]? requestFilterProjectTypes)
        {
            var baseUrl = "/api/v1/summary/project";

            var queryParams = new Dictionary<string, string?>
            {
                { "projectManagedByEmail", userEmail }
            };

            var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

            var result = await Get<ApiListWrapper<GetProjectSummaryResponse>>(url);

            var output = result.Data.Where(x => x.ProjectId.Any() &&
                                                            FilterProjectStatus(x, requestFilterProjectTypes))
                .Select(x => new MfspSummary()
            {
                ProjectId = x.ProjectId,
                ProjectType = x.ProjectType,
                CurrentName = x.ProjectTitle,
                TrustName = x.TrustName,
                SchoolType = x.SchoolType,
                ProjectStatus = x.ProjectStatus,
                ProjectManagedBy = x.ProjectManagedBy,
                RealisticYearOfOpening = x.RealisticOpeningYear,
                LocalAuthority = x.LocalAuthority,
                UpdatedAt = x.UpdatedAt,
                Region = x.Region,
                ProjectManagedByEmail = x.ProjectManagedByEmail
            });
            
            return output;
        }

        private static bool FilterProjectStatus(GetProjectSummaryResponse response, string[]? filters)
        {
            if (filters is not { Length: > 0})
            {
                return true;
            }

            return filters is { Length: > 0 } && filters.Contains(response.ProjectStatus);
        }

    }
}
