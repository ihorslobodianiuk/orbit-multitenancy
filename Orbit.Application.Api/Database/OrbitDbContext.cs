using Microsoft.EntityFrameworkCore;
using Orbit.Application.Api.Database.Mappings;
using Orbit.Application.Api.Middleware;
using Orbit.Application.Api.Models;

namespace Orbit.Application.Api.Database
{
    public class OrbitDbContext : DbContext
    {
        private readonly IDomainContextInfo _domainContextInfo;
    
        public DbSet<Product> Products { get; set; }

        public OrbitDbContext(DbContextOptions<OrbitDbContext> options, IDomainContextInfo domainContextInfo) 
            : base(options)
        {
            _domainContextInfo = domainContextInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_domainContextInfo.TenantId.HasValue)
            {
                var connection = GetConnection(_domainContextInfo.TenantId.Value);
                optionsBuilder.UseNpgsql(connection);
            }
        }

        private string GetConnection(Guid tenantId)
        {
            return $"Server=localhost;Port=5432;Database=orbit-test;User Id={tenantId};Password=123;";
        }
    }
}