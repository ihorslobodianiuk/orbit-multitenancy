using Microsoft.Extensions.Options;
using Orbit.Application.Api.Dto;
using Orbit.Application.Api.Infrastructure;

namespace Orbit.Application.Api.Services;

public class TenantService : ITenantService
{
    private readonly HttpClient _httpClient;
    private readonly TenantApiOptions _apiOptions;

    public TenantService(HttpClient httpClient,
        IOptions<TenantApiOptions> apiOptions)
    {
        _apiOptions = apiOptions.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_apiOptions.BaseUrl);
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