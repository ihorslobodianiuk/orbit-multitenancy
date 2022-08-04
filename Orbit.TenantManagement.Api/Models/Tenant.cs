namespace Orbit.TenantManagement.Api.Models;

public class Tenant
{
    public Tenant()
    {
        TenantFeatures = new HashSet<TenantFeature>();
    }

    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Tier { get; set; }

    public virtual ICollection<TenantFeature> TenantFeatures { get; set; }
}