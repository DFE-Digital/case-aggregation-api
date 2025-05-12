using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using AutoMapper;

namespace Dfe.CaseAggregationService.Application.MappingProfiles
{
    public class TransferInfoProfile: Profile
    {
        public TransferInfoProfile()
        {
            CreateMap<AcademisationSummary, TransfersCaseInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Urn, opt => opt.MapFrom(src => src.Urn))
                .ForMember(dest => dest.LastModifiedOn, opt => opt.MapFrom(src => src.LastModifiedOn))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.ProjectReference,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.ProjectReference : null))
                .ForMember(dest => dest.OutgoingTrustUkprn,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.OutgoingTrustUkprn : null))
                .ForMember(dest => dest.OutgoingTrustName,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.OutgoingTrustName : null))
                .ForMember(dest => dest.TypeOfTransfer,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.TypeOfTransfer : null))
                .ForMember(dest => dest.TargetDateForTransfer,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.TargetDateForTransfer : null))
                .ForMember(dest => dest.AssignedUserEmailAddress,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.AssignedUserEmailAddress : null))
                .ForMember(dest => dest.AssignedUserFullName,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.AssignedUserFullName : null))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.TransfersSummary != null ? src.TransfersSummary.Status : null))
                .ForMember(dest => dest.IncomingTrustUkprn,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.IncomingTrustUkprn : null))
                .ForMember(dest => dest.IncomingTrustName,
                    opt => opt.MapFrom(src =>
                        src.TransfersSummary != null ? src.TransfersSummary.IncomingTrustName : null));
        }
    }
}
