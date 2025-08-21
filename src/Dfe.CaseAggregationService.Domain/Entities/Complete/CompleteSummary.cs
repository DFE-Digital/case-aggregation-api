using System.ComponentModel;

namespace Dfe.CaseAggregationService.Domain.Entities.Complete
{
    public enum CompleteProjectType
    {
        Unknown,
        [Description("Conversion")]
        Conversion,
        [Description("Transfer")]
        Transfer,
        [Description("Form A MAT Conversion")]
        FormAMatConversion,
        [Description("Form A MAT Transfer")]
        FormAMatTransfer,
    }
    public record CompleteSummary(
        Guid? ProjectId,
        CompleteProjectType CaseType,
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
