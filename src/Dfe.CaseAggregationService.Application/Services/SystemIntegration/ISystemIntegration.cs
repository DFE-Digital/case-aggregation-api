using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public interface ISystemIntegration
    {
        Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken);
    }
}
