using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.SignificantChange.Client.Contracts;

namespace Dfe.CaseAggregationService.Application.MappingProfiles
{
    public class SignificantChangeProfile: Profile
    {
        public SignificantChangeProfile()
        {
            CreateMap<SignificantChangeCase, SignificantChangeCaseInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Urn, opt => opt.MapFrom(src => src.Urn))
                .ForMember(dest => dest.CaseType, opt => opt.MapFrom(src => src.CaseType))
                .ForMember(dest => dest.Academy, opt => opt.MapFrom(src => src.Academy))
                .ForMember(dest => dest.Trust, opt => opt.MapFrom(src => src.Trust))
                .ForMember(dest => dest.LA, opt => opt.MapFrom(src => src.La))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.DateOfDecision, opt => opt.MapFrom(src => src.DateOfDecision))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        }
    }
}

/*
         public int Id { get; set; }
   public int? Urn { get; set; }
   public string? CaseType { get; set; }
   public string? Academy { get; set; }
   public string? Trust { get; set; }
   public string? LA { get; set; }
   public string? Region { get; set; }

   public DateTime? DateOfDecision { get; set; }
   public string? Username { get; set; }
*/