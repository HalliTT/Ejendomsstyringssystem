using AuthService.Domain.Addresses;
using AuthService.Domain.Grants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class GrantConfiguration : IEntityTypeConfiguration<Grant>
    {
        public void Configure(EntityTypeBuilder<Grant> builder)
        {
            builder.ToTable("grants");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.CodeHash)
                .HasColumnName("code")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(x => x.GrantType)
                .HasColumnName("grant_type")
                .HasConversion<short>()
                .HasMaxLength(255);

            builder.Property(x => x.RedirectUri)
                .HasColumnName("redirect_uri")
                .HasColumnType("Text");

            builder.Property(x => x.Scopes)
                .HasColumnName("scopes")
                .HasColumnType("Text");

            builder.Property(x => x.CodeChallange)
                .HasColumnName("code_challenge")
                .HasMaxLength(255);

            builder.Property(x => x.CodeChallengeMethod)
                .HasColumnName("code_challenge_method")
                .HasMaxLength(10);

            builder.Property(x => x.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(x => x.ExpiresAt)
               .HasColumnName("expires_at")
               .IsRequired();

            builder.Property(x => x.ConsumedAt)
               .HasColumnName("consumed_at");

            builder.Property(x => x.ApplicationId)
               .HasColumnName("application_id");

            builder.Property(x => x.UserId)
               .HasColumnName("user_id");

            builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at");

            builder.Property(x => x.Version)
                .IsRowVersion();

            builder.HasOne(x => x.Application)
                .WithMany(x => x.Grants)
                .HasForeignKey(x => x.ApplicationId);
        }

    }
}
