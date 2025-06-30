using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Domain.Entities.Recast
{
    public class RecastSummary
    {
        public string System { get; set; } = string.Empty;
        public string CaseType { get; set; } = string.Empty;
        public string TrustName { get; set; } = string.Empty;
        public string Trn { get; set; } = string.Empty;
        public DateTime DateCaseCreated { get; set; }
        public string RiskToTrust { get; set; } = string.Empty;
        public string DirectionOfTravel { get; set; } = string.Empty;

    }
}
