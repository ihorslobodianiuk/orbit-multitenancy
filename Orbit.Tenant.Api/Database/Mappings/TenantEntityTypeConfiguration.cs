using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Tenant.Api.Models;

namespace Orbit.Tenant.Api.Database.Mappings;

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Models.Tenant>
{
    public void Configure(EntityTypeBuilder<Models.Tenant> builder)
    {
        builder.ToTable("tenant");

        builder.HasIndex(e => e.Name, "tenant_name_key")
            .IsUnique();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");

        builder.Property(e => e.Status)
            .HasMaxLength(64)
            .HasColumnName("status");

        builder.Property(e => e.Tier)
            .HasMaxLength(64)
            .HasColumnName("tier");

        builder.HasMany(d => d.Features)
            .WithMany(p => p.Tenants)
            .UsingEntity<Dictionary<string, object>>(
                "TenantFeature",
                l => l.HasOne<Feature>().WithMany().HasForeignKey("FeatureId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("tenant_feature_feature_id_fkey"),
                r => r.HasOne<Models.Tenant>().WithMany().HasForeignKey("TenantId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("tenant_feature_tenant_id_fkey"),
                j =>
                {
                    j.HasKey("TenantId", "FeatureId").HasName("tenant_feature_pkey");

                    j.ToTable("tenant_feature");

                    j.IndexerProperty<Guid>("TenantId").HasColumnName("tenant_id");

                    j.IndexerProperty<int>("FeatureId").HasColumnName("feature_id");
                });
    }
}