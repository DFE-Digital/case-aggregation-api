using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Complete;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class CompleteIntegration(ICompleteRepository repo,
        IGetCaseInfo<CompleteSummary> mapper,
        ILogger<CompleteIntegration> logger)
        : ISystemIntegration
    {
        
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {
            if (query.IncludeComplete)
            {
                return repo.GetCompleteSummaryForUser(query.UserEmail, cancellationToken)
                    .ContinueWith(ProcessComplete, cancellationToken);
            }

            return Task.FromResult<IEnumerable<UserCaseInfo>>(new List<UserCaseInfo>());
        }

        private IEnumerable<UserCaseInfo> ProcessComplete(Task<IEnumerable<CompleteSummary>> cases)
        {
            if (!cases.IsFaulted)
            {
                var recast = cases.Result.ToList();
                return recast.Select(mapper.GetCaseInfo);
            }
            
            if (cases.Exception is not null)
                logger.LogError(cases.Exception, cases.Exception.Message);
            
            return [];
        }
    }
}
