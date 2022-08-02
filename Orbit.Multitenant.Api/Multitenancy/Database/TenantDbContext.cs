// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Orbit.Multitenant.Api.Multitenancy.Database.Mappings;
using Orbit.Multitenant.Api.Multitenancy.Models;

namespace Orbit.Multitenant.Api.Multitenancy.Database
{
    /// <summary>
    /// A DbContext to access the Tenant Database.
    /// </summary>
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Tenants.
        /// </summary>
        public DbSet<Tenant> Tenants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}