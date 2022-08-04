using AutoMapper;
using Orbit.Multitenant.Api.Dto;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Infrastructure;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tenant, TenantDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Feature, FeatureDto>();
    }
}