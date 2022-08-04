using Orbit.Application.Api.Dto;
using Orbit.Application.Api.Infrastructure;

namespace Orbit.Application.Api.Services;

public class UserService : IUserService  
{
    private readonly ITenantService _tenantService;

    public UserService(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }
        
    public async Task<TenantDto> ValidateCredentials(string username, string password)
    {
        var tenant = await _tenantService.GetTenantByName(username);
        if (tenant == null || password != Constants.DefaultPassword)
            throw new ArgumentException("Invalid credentials");
        return tenant;
    }  
}

public interface IUserService
{
    Task<TenantDto> ValidateCredentials(string username, string password);
}