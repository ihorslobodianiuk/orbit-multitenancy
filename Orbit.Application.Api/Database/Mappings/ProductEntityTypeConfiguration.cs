using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Application.Api.Models;

namespace Orbit.Application.Api.Database.Mappings
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .ToTable("product", "public")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("product_id");

            builder
                .Property(x => x.Name)
                .HasColumnName("name");
        }
    }
}