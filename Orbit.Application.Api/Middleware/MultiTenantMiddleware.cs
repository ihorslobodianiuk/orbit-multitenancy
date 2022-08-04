﻿using Orbit.Application.Api.Infrastructure;

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
                var tenantValue = context.User.Claims.FirstOrDefault(c => c.Type == Constants.TenantClaim)?.Value;
                if (!string.IsNullOrWhiteSpace(tenantValue) && Guid.TryParse(tenantValue, out var tenantId))
                {
                    domainContextInfo.TenantId = tenantId;
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid Tenant", context.RequestAborted);
                    return;
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