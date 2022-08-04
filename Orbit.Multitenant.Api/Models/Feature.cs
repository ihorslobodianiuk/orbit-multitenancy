namespace Orbit.Multitenant.Api.Models;

public class Feature
{
    public Feature()
    {
        Tenants = new HashSet<Tenant>();
    }

    public int FeatureId { get; set; }
    public string? Name { get; set; }

    public ICollection<Tenant> Tenants { get; set; }
    
    public bool Enabled => Tenants.Any();

    public void Toggle(Guid tenantId)
    {
        if (Enabled)
        {
            Tenants.Remove(Tenants.First(t => t.TenantId == tenantId));
        }
        else
        {
            Tenants.Add(new Tenant { TenantId = tenantId });
        }
    }
}