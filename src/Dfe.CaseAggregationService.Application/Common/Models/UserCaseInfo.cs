using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Application.Common.Models
{
    public class UserCaseInfo
    {
        public List<SignificantChangeCaseInfo> SignificantChangeCases { get; set; } = new List<SignificantChangeCaseInfo>();
        public List<ConversionsCaseInfo> ConversionSummaries { get; set; } = new List<ConversionsCaseInfo>();

        public List<TransfersCaseInfo> TransfersSummaries { get; set; } = new List<TransfersCaseInfo>();
    }

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

    public class TransfersCaseInfo
    {
        public int Id { get; set; }
        public int Urn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? ProjectReference { get; set; }
        public string? OutgoingTrustUkprn { get; set; }
        public string? OutgoingTrustName { get; set; }
        public string? TypeOfTransfer { get; set; }
        public DateTime? TargetDateForTransfer { get; set; }
        public string? AssignedUserEmailAddress { get; set; }
        public string? AssignedUserFullName { get; set; }
        public string? Status { get; set; }
        public string? IncomingTrustUkprn { get; set; }
        public string? IncomingTrustName { get; set; }
    }


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
}
