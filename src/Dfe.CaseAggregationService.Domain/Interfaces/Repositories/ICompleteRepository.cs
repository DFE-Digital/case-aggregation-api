using Dfe.CaseAggregationService.Domain.Entities.Complete;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface ICompleteRepository
    {
        Task<IEnumerable<CompleteSummary>> GetCompleteSummaryForUser(string userEmail,
            CancellationToken cancellationToken);
    }
}
