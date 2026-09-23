using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    internal class ApplicationRedirectUrisConfiguration : IEntityTypeConfiguration<Domain.Applications.ApplicationRedirectUris>
    {
        public void Configure(EntityTypeBuilder<Domain.Applications.ApplicationRedirectUris> builder)
        {
            builder.ToTable("applications_redirect_uris");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.ApplicationId)
                .HasColumnName("application_id");

            builder.Property(x => x.RedirectUris)
                .HasColumnName("redirect_uris")
                .HasColumnType("Text");

            builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

            builder.HasOne(aru => aru.Applications)
                .WithMany(a => a.ApplicationRedirectUrises)
                .HasForeignKey(aru => aru.ApplicationId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
