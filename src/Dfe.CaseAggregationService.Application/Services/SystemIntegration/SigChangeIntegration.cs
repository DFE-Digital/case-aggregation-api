using Microsoft.Extensions.Logging;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.SigChange;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class SigChangeIntegration(
        ISigChangeRepository repo,
        IGetCaseInfo<SigChangeSummary> mapper,
        ILogger<SigChangeIntegration> logger)
        : IntegrationWrapper<SigChangeSummary>(mapper, logger), ISystemIntegration
    {
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {
            if (query.FilterProjectTypes != null && query.FilterProjectTypes.Length > 0)
            {
                return SkippedResult("SigChange");
            }

            if (query.IncludeSignificantChange)
            {
                return TrackAndProcess(
                    "SigChange",
                    () => repo.GetSigChangeSummaries(query.UserName, query.RecordCount, cancellationToken),
                    cancellationToken);
            }

            return SkippedResult("SigChange");
        }
    }
}
