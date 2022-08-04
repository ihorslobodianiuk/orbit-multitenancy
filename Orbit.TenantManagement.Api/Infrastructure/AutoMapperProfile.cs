using AutoMapper;
using Orbit.TenantManagement.Api.Dto;
using Orbit.TenantManagement.Api.Models;

namespace Orbit.TenantManagement.Api.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tenant, TenantDto>().ReverseMap();
        CreateMap<TenantPostDto, Tenant>()
            .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<Feature, FeatureDto>();
    }
}