using AutoMapper;
using Orbit.Application.Api.Dto;
using Orbit.Application.Api.Models;

namespace Orbit.Application.Api.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}