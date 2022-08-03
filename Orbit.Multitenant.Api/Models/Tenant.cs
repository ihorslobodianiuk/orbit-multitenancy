namespace Orbit.Multitenant.Api.Database.Models;

public class Tenant
{
    public Guid TenantId { get; set; }

    public string Name { get; set; }
}