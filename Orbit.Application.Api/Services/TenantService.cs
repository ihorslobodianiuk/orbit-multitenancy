using Orbit.Application.Api.Dto;

namespace Orbit.Application.Api.Services;

public class TenantService : ITenantService
{
    private readonly HttpClient _httpClient;

    public TenantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<TenantDto> GetTenantByName(string tenantName)
    {
        return _httpClient.GetFromJsonAsync<TenantDto>($"Tenant/{tenantName}");
    }

    public Task<IEnumerable<FeatureDto>> GetFeatures(Guid tenantId)
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<FeatureDto>>($"Feature/{tenantId.ToString()}");
    }
}

public interface ITenantService
{
    Task<TenantDto> GetTenantByName(string tenantName);
    Task<IEnumerable<FeatureDto>> GetFeatures(Guid tenantId);
}