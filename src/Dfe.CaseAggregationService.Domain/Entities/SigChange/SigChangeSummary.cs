namespace Dfe.CaseAggregationService.Domain.Entities.SigChange;

public class SigChangeSummary
{
    public string SigChangeId { get; set; }
    public string? AcademyName { get; set; }
    public string? ChangeType { get; set; }
    public string? Trust { get; set; }
    public string? Urn { get; set; }
    public string? LocalAuthority { get; set; }
    public string? Region { get; set; }
    public DateTime? DateOfDecision { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}