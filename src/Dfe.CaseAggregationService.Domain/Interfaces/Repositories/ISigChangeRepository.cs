using Dfe.CaseAggregationService.Domain.Entities.SigChange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface ISigChangeRepository
    {
        Task<IEnumerable<SigChangeSummary>> GetSigChangeSummaries(string? userName, CancellationToken cancellationToken);
    }
}
