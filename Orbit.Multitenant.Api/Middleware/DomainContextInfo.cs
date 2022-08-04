namespace Orbit.Multitenant.Api.Middleware;

internal class DomainContextInfo : IDomainContextInfo
{
    public Guid? TenantId { get; set; }
    public string? TenantName => TenantId?.ToString();
}