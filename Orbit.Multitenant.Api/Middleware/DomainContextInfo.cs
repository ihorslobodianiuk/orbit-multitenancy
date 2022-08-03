namespace Orbit.Multitenant.Api.Middleware;

internal class DomainContextInfo : IDomainContextInfo
{
    public string TenantName { get; set; }
}