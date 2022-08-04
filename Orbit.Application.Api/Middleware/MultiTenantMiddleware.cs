using Microsoft.FeatureManagement;
using Orbit.Application.Api.Infrastructure;
using Orbit.Application.Api.Services;

namespace Orbit.Application.Api.Middleware
{
    public class MultiTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            IDomainContextInfo domainContextInfo,
            ITenantService tenantService)
        {
            if (context?.User.Identity?.IsAuthenticated == true)
            {
                var tenantClaim = context.User.Claims.FirstOrDefault(c => c.Type == Constants.TenantClaim)?.Value;
                if (string.IsNullOrWhiteSpace(tenantClaim))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid Tenant", context.RequestAborted);
                    return;
                }
                
                var tenant = await tenantService.GetTenantByName(tenantClaim);
                domainContextInfo.TenantId = tenant.TenantId;
                domainContextInfo.Features = await tenantService.GetFeatures(tenant.TenantId);
            }

            await _next(context);
        }
    }

    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenantMiddleware>();
        }
    }
}