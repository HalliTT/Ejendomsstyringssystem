using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Domain.Tenants.Tenant>
    {
        public void Configure(EntityTypeBuilder<Domain.Tenants.Tenant> builder)
        {
            builder.ToTable("tenants");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
               .HasColumnName("name")
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255);

            builder.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(50);

            builder.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .HasDefaultValue(true);

            builder.Property(x => x.SoftDeletedAt)
                .HasColumnName("soft_deleted_at");

            builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");
        }
    }
}