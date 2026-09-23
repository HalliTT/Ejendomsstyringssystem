using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Applications.Application>
    {
        public void Configure(EntityTypeBuilder<Domain.Applications.Application> builder)
        {
            builder.ToTable("applications");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            builder.Property(x => x.OrganizationId)
                .HasColumnName("organization_id");

            builder.Property(x => x.ClientId)
                .HasColumnName("client_id");

            builder.Property(x => x.ClientSecret)
                .HasColumnName("client_secret")
                .HasMaxLength(255);

            builder.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .HasDefaultValue(true);

            builder.Property(x => x.SoftDeletedAt)
                .HasColumnName("soft_deleted_at");

            builder.Property(x => x.DeletedBy)
                .HasColumnName("deleted_by");

            builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

            builder.HasMany(a => a.ApplicationRedirectUrises)
                .WithOne(aru => aru.Applications)
                .HasForeignKey(aru => aru.ApplicationId);

            builder.HasOne(a => a.DeletedByUser)
                .WithMany()
                .HasForeignKey(a => a.DeletedBy)
                .IsRequired(false);
        }
    }
}
