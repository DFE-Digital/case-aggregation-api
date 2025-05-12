using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Services
{
    public interface IGetAcademisationSummary
    {
        Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail);
    }
}
