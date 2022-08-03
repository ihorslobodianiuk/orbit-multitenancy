using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Orbit.Multitenant.Api.Database.Mappings;
using Orbit.Multitenant.Api.Database.Models;
using Orbit.Multitenant.Api.Middleware;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Database;

public class OrbitDbContext : DbContext
{
    private readonly IDomainContextInfo _domainContextInfo;
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    public OrbitDbContext(DbContextOptions<OrbitDbContext> options, IDomainContextInfo domainContextInfo) 
        : base(options)
    {
        _domainContextInfo = domainContextInfo;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(_domainContextInfo.TenantName))
        {
            optionsBuilder.UseNpgsql(option => GetConnection(_domainContextInfo.TenantName));
        }
    }

    private string? GetConnection(string tenantName)
    {
        var connection = this.Database.GetConnectionString();
        return connection == null || connection.Contains("Username") ? connection : connection + $"Username={tenantName};Password=123;";
    }
}