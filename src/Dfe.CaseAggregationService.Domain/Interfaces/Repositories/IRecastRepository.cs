using Dfe.CaseAggregationService.Domain.Entities.Recast;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface IRecastRepository
    {
        Task<IEnumerable<RecastSummary>> GetRecastSummaries(string userEmail);
    }
}
