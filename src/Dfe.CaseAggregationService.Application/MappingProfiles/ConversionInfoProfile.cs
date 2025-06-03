using AutoMapper;
using Dfe.CaseAggregationService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Application.MappingProfiles
{
    public class ConversionInfoProfile : Profile
    {
        public ConversionInfoProfile()
        {
            CreateMap<AcademisationSummary, ConversionsCaseInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Urn, opt => opt.MapFrom(src => src.Urn))
                .ForMember(dest => dest.LastModifiedOn, opt => opt.MapFrom(src => src.LastModifiedOn))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.ApplicationReferenceNumber,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.ApplicationReferenceNumber : null))
                .ForMember(dest => dest.SchoolName,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.SchoolName : null))
                .ForMember(dest => dest.LocalAuthority,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.LocalAuthority : null))
                .ForMember(dest => dest.Region,
                    opt => opt.MapFrom(src => src.ConversionsSummary != null ? src.ConversionsSummary.Region : null))
                .ForMember(dest => dest.AcademyTypeAndRoute,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.AcademyTypeAndRoute : null))
                .ForMember(dest => dest.NameOfTrust,
                    opt =>
                        opt.MapFrom(src => src.ConversionsSummary != null ? src.ConversionsSummary.NameOfTrust : null))
                .ForMember(dest => dest.AssignedUserEmailAddress,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.AssignedUserEmailAddress : null))
                .ForMember(dest => dest.AssignedUserFullName,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.AssignedUserFullName : null))
                .ForMember(dest => dest.ProjectStatus,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.ProjectStatus : null))
                .ForMember(dest => dest.TrustReferenceNumber,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.TrustReferenceNumber : null))
                .ForMember(dest => dest.CreatedOn,
                    opt => opt.MapFrom(src => src.ConversionsSummary != null ? src.ConversionsSummary.CreatedOn : null))
                .ForMember(dest => dest.Decision,
                    opt => opt.MapFrom(src => src.ConversionsSummary != null ? src.ConversionsSummary.Decision : null))
                .ForMember(dest => dest.ConversionTransferDate,
                    opt => opt.MapFrom(src =>
                        src.ConversionsSummary != null ? src.ConversionsSummary.ConversionTransferDate : null));
        }
    }
}
