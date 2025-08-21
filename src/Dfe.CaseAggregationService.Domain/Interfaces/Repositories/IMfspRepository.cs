using Dfe.CaseAggregationService.Domain.Entities.Mfsp;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface IMfspRepository
    {
        Task<IEnumerable<MfspSummary>> GetMfspSummaries(string userEmail, string[]? requestFilterProjectTypes);
    }
}
