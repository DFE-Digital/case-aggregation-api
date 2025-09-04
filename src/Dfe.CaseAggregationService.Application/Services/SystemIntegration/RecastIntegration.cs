using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Recast;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class RecastIntegration(
        IRecastRepository repo,
        IGetCaseInfo<RecastSummary> mapper,
        ILogger<RecastIntegration> logger)
        : IntegrationWrapper<RecastSummary>(mapper, logger), ISystemIntegration
    {
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {

            if (query.IncludeConcerns)
            {
                return repo.GetRecastSummaries(query.UserEmail, query.FilterProjectTypes)
                    .ContinueWith(ProcessResult, cancellationToken);
            }

            return Task.FromResult<IEnumerable<UserCaseInfo>>(new List<UserCaseInfo>());
        }
    }
}
