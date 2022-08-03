using Orbit.Multitenant.Api.Dto;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Converters;

public static class CustomerConverter
{
    public static Customer ToModel(CustomerDto source)
    {
        return new Customer
        {
            Id = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
        };
    }

    public static CustomerDto ToDto(Customer source)
    {
        return new CustomerDto
        {
            Id = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
        };
    }
}