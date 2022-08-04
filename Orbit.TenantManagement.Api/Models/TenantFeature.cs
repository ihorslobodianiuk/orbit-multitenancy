namespace Orbit.TenantManagement.Api.Models;

public class TenantFeature
{
    public Guid TenantId { get; set; }
    public int FeatureId { get; set; }
    public bool Enabled { get; set; }
    public string Config { get; set; }

    public virtual Feature Feature { get; set; }
    public virtual Tenant Tenant { get; set; }
}