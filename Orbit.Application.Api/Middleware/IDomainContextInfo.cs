using Orbit.Application.Api.Dto;

namespace Orbit.Application.Api.Middleware
{
    public interface IDomainContextInfo
    {
        Guid? TenantId { get; set; }
        IEnumerable<FeatureDto> Features { get; set; }
    }
}