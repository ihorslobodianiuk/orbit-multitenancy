using Orbit.Application.Api.Dto;

namespace Orbit.Application.Api.Middleware
{
    internal class DomainContextInfo : IDomainContextInfo
    {
        public Guid? TenantId { get; set; }
        public IEnumerable<FeatureDto> Features { get; set; }
    }
}