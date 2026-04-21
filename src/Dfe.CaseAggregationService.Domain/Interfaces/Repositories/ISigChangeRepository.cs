using Dfe.CaseAggregationService.Domain.Entities.SigChange;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface ISigChangeRepository
    {
        Task<IEnumerable<SigChangeSummary>> GetSigChangeSummaries(string? userName, int recordCount, CancellationToken cancellationToken);
    }
}
