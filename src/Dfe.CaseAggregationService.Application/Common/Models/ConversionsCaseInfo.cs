namespace Dfe.CaseAggregationService.Application.Common.Models;

public class ConversionsCaseInfo
{
    public int Id { get; set; }
    public int Urn { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string? ApplicationReferenceNumber { get; set; }
    public string? SchoolName { get; set; }
    public string? LocalAuthority { get; set; }
    public string? Region { get; set; }
    public string? AcademyTypeAndRoute { get; set; }
    public string? NameOfTrust { get; set; }
    public string? AssignedUserEmailAddress { get; set; }
    public string? AssignedUserFullName { get; set; }
    public string? ProjectStatus { get; set; }
    public string? TrustReferenceNumber { get; set; }
    public string? Decision { get; set; }
    public DateTime? ConversionTransferDate { get; set; }
}