// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Multitenant.Api.Models;

namespace Orbit.Multitenant.Api.Database.Mappings
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .ToTable("customer", "sample")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("customer_id");

            builder
                .Property(x => x.FirstName)
                .HasColumnName("first_name");

            builder
                .Property(x => x.LastName)
                .HasColumnName("last_name");
        }
    }
}
