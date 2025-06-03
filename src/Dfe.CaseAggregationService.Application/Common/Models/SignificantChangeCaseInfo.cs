namespace Dfe.CaseAggregationService.Application.Common.Models;

public class SignificantChangeCaseInfo
{
    public int Id { get; set; }
    public int? Urn { get; set; }
    public string? CaseType { get; set; }
    public string? Academy { get; set; }
    public string? Trust { get; set; }
    public string? LA { get; set; }
    public string? Region { get; set; }

    public DateTime? DateOfDecision { get; set; }
    public string? Username { get; set; }
}