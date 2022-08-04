namespace Orbit.Application.Api.Middleware
{
    public interface IDomainContextInfo
    {
        Guid? TenantId { get; set; }
        public string TenantName { get; }
    }
}