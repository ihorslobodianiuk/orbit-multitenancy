// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Orbit.Multitenant.Api.Multitenancy.Models;

namespace Orbit.Multitenant.Api.Multitenancy.Context
{
    /// <summary>
    /// Holds the Tenant in an Ambient Context, which simplifies the code.
    /// </summary>
    public static class TenantExecutionContext
    {
        /// <summary>
        /// Holds the Tenant in an <see cref="AsyncLocal{T}"/>, so it flows top-down.
        /// </summary>
        private static AsyncLocal<Tenant> _tenant = new AsyncLocal<Tenant>();

        /// <summary>
        /// Gets the current Tenant 
        /// </summary>
        public static Tenant? Tenant => _tenant.Value;

        public static void SetTenant(Tenant value)
        {
            if(value == null)
            {
                throw new InvalidOperationException($"Tried set an empty Tenant");
            }

            var currentTenant = _tenant.Value;

            if(currentTenant == null || string.Equals(currentTenant.Name, value.Name, StringComparison.InvariantCulture))
            {
                _tenant.Value = value;

                return;
            }

            throw new InvalidOperationException($"Tried assign the Tenant to '{value.Name}', but it is already set to {currentTenant.Name}");
       }
    }
}
