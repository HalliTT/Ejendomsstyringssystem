using AuthService.Domain.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.AddressLine1)
                .HasColumnName("address_line_one")
                .HasMaxLength(255);

            builder.Property(x => x.AddressLine2)
                .HasColumnName("address_line_two")
                .HasMaxLength(255);

            builder.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(255);

            builder.Property(x => x.State)
                .HasColumnName("state")
                .HasMaxLength(255);

            builder.Property(x => x.Zip)
                .HasColumnName("zip")
                .HasMaxLength(255);

            builder.Property(x => x.Country)
                .HasColumnName("country")
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()");
        }
    }
}
