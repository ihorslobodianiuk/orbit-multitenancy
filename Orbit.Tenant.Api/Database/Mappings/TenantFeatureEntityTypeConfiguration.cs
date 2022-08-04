using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Tenant.Api.Models;

namespace Orbit.Tenant.Api.Database.Mappings;

public class TenantFeatureEntityTypeConfiguration : IEntityTypeConfiguration<TenantFeature>
{
    public void Configure(EntityTypeBuilder<TenantFeature> builder)
    {
        builder.HasKey(e => new { e.TenantId, e.FeatureId })
            .HasName("tenant_feature_pkey");

        builder.ToTable("tenant_feature");

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");

        builder.Property(e => e.FeatureId).HasColumnName("feature_id");

        builder.Property(e => e.Config)
            .HasColumnType("jsonb")
            .HasColumnName("config");

        builder.Property(e => e.Enabled)
            .IsRequired()
            .HasColumnType("boolean")
            .HasColumnName("enabled");

        builder.HasOne(d => d.Feature)
            .WithMany(p => p.TenantFeatures)
            .HasForeignKey(d => d.FeatureId)
            .HasConstraintName("tenant_feature_feature_id_fkey");

        builder.HasOne(d => d.Tenant)
            .WithMany(p => p.TenantFeatures)
            .HasForeignKey(d => d.TenantId)
            .HasConstraintName("tenant_feature_tenant_id_fkey");
    }
}