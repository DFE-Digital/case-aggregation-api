using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface IAcademisationRepository
    {
        Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm);
    }
}
