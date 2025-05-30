using AutoMapper;
using Core.Models;
using DomainLayer.Models;
using Shared.DataTransferObject;
using System;

namespace Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // BlockedAttemptLog <-> BlockedAttemptDto
            CreateMap<BlockedAttemptLog, BlockedAttemptDto>()
                .ForMember(dest => dest.BlockedStatus, opt => opt.MapFrom(src => src.IsBlocked));
            CreateMap<BlockedAttemptDto, BlockedAttemptLog>()
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.BlockedStatus));
            
            // BlockedCountry <-> BlockCountryDto
            CreateMap<BlockedCountry, BlockCountryDto>()
                .ForMember(dest => dest.BlockDurationMinutes, opt => opt.MapFrom(src => 
                    src.ExpiresAt.HasValue ? (int?)Math.Ceiling((src.ExpiresAt.Value - DateTime.UtcNow).TotalMinutes) : null));
            CreateMap<BlockCountryDto, BlockedCountry>()
                .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => 
                    src.BlockDurationMinutes.HasValue ? DateTime.UtcNow.AddMinutes(src.BlockDurationMinutes.Value) : (DateTime?)null));
            
            // TemporalBlock <-> TemporalBlockRequest
            CreateMap<TemporalBlockRequest, TemporalBlock>()
                .ForMember(dest => dest.UnblockAt, opt => opt.MapFrom(src => 
                    DateTime.UtcNow.AddMinutes(src.DurationMinutes)));
                    
            // BlockCountryDto -> BlockedCountryResponse
            CreateMap<BlockCountryDto, BlockedCountryResponse>();
        }
    }
}
