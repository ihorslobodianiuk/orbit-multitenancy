namespace Orbit.TenantManagement.Api.Dto;

public class TenantFeatureDto
{
    public int FeatureId { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string Config { get; set; }
}