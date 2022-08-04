namespace Orbit.Tenant.Api.Models;

public class Tenant
{
    public Tenant()
    {
        Features = new HashSet<Feature>();
    }

    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Tier { get; set; }

    public virtual ICollection<Feature> Features { get; set; }
}