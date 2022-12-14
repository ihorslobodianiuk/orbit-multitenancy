namespace Orbit.TenantManagement.Api.Models;

public class Feature
{
    public Feature()
    {
        TenantFeatures = new HashSet<TenantFeature>();
    }

    public int FeatureId { get; set; }
    public string? Name { get; set; }

    public ICollection<TenantFeature> TenantFeatures { get; set; }
}