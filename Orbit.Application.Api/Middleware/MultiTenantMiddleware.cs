using System.Security.Claims;

namespace Orbit.Application.Api.Middleware
{
    public class MultiTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDomainContextInfo domainContextInfo)
        {
            if (context?.User.Identity?.IsAuthenticated == true)
            {
                var tenantValue = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (!string.IsNullOrWhiteSpace(tenantValue) && Guid.TryParse(tenantValue, out var tenantId))
                {
                    domainContextInfo.TenantId = tenantId;
                    // var tenant = await dbContext.Tenants
                    //     .AsNoTracking()
                    //     .FirstOrDefaultAsync(x => x.Name == tenantNameString, context.RequestAborted);
                    //
                    // if (tenant == null)
                    // {
                    //     context.Response.StatusCode = 400;
                    //     await context.Response.WriteAsync("Invalid Tenant Name", context.RequestAborted);
                    //
                    //     return;
                    // }
                }
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