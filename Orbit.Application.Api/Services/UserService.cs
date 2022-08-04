namespace Orbit.Application.Api.Services
{
    public class UserService : IUserService  
    {
        public bool ValidateCredentials(string username, string password)  
        {
            // We should actually validate tenant name and password properly here but for demo purpose just pass due to Db initialization
            // var tenant = _context.Tenants.FirstOrDefault(t => t.Name == username);
            // return tenant != null && password.Equals("123");  
            return Guid.TryParse(username, out var _) && password.Equals("123");  
        }  
    }

    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}