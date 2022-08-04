using AutoMapper;
using Orbit.Tenant.Api.Dto;
using Orbit.Tenant.Api.Models;

namespace Orbit.Tenant.Api.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Models.Tenant, TenantDto>().ReverseMap();
        CreateMap<Feature, FeatureDto>();
    }
}