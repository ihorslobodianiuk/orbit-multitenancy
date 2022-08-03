using Orbit.Multitenant.Api.Dto;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Converters;

public static class ProductConverter
{
    public static Product ToModel(ProductDto source)
    {
        return new Product
        {
            Id = source.Id,
            Name = source.Name,
        };
    }

    public static ProductDto ToDto(Product source)
    {
        return new ProductDto
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}