using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Domain.Entities.Complete
{
    public record CompleteSummary(
        Guid? ProjectId,
        string CaseType,
        string AcademyName,
        string Urn,
        DateTime? ProposedTransferDate,
        string IncomingTrust,
        string OutgoingTrust,
        string LocalAuthority,
        DateTime? CreatedDate,
        DateTime? UpdatedDate
    );
}
