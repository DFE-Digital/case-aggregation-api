using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Complete;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class CompleteIntegration(
        ICompleteRepository repo,
        IGetCaseInfo<CompleteSummary> mapper,
        ILogger<CompleteIntegration> logger)
        : IntegrationWrapper<CompleteSummary>(mapper, logger), ISystemIntegration
    {
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {
            if (query.IncludeComplete)
            {
                return repo.GetCompleteSummaryForUser(query.UserEmail, query.FilterProjectTypes, cancellationToken)
                                .ContinueWith(ProcessResult, cancellationToken);
            }

            return Task.FromResult<IEnumerable<UserCaseInfo>>(new List<UserCaseInfo>());
        }
    }
}
