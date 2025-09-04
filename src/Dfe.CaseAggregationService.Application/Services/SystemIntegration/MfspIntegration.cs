using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class MfspIntegration(
        IMfspRepository repo,
        IGetCaseInfo<MfspSummary> mapper,
        ILogger<MfspIntegration> logger)
        : IntegrationWrapper<MfspSummary>(mapper, logger), ISystemIntegration
    {
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {
            if (query.IncludeManageFreeSchools)
            {
                return repo.GetMfspSummaries(query.UserEmail, query.FilterProjectTypes)
                    .ContinueWith(ProcessResult, cancellationToken);
            }

            return Task.FromResult<IEnumerable<UserCaseInfo>>(new List<UserCaseInfo>());
        }
    }
}
