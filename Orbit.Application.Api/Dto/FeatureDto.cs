namespace Orbit.Application.Api.Dto;

public class FeatureDto
{
    public int FeatureId { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string Config { get; set; }
}