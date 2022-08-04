namespace Orbit.TenantManagement.Api.Dto;

public class TenantFeaturePostDto
{
    public Guid TenantId { get; set; }
    public int FeatureId { get; set; }
    public bool Enabled { get; set; }
    public string Config { get; set; }
}