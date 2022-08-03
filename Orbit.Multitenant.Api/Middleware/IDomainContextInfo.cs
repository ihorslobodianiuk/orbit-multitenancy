namespace Orbit.Multitenant.Api.Middleware;

public interface IDomainContextInfo
{
    string TenantName { get; set; }
}