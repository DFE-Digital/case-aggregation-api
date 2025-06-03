using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Services
{
    public interface IGetAcademisationSummary
    {
        Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm);
    }
}
