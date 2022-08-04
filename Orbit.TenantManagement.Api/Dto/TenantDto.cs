namespace Orbit.TenantManagement.Api.Dto;

public class TenantDto
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
}

public class TenantPostDto
{
    public string Name { get; set; }
}