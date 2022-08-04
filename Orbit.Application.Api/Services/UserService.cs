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
        
    public bool ValidateCredentials(string username, string password)
    {
        return !string.IsNullOrEmpty(username) && password == Constants.DefaultPassword;
    }  
}

public interface IUserService
{
    bool ValidateCredentials(string username, string password);
}