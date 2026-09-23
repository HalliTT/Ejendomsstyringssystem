using AuthService.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
    {
        public void Configure(EntityTypeBuilder<AccessToken> builder)
        {
            builder.ToTable("access_tokens");

            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Token)
                .HasColumnName("token")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.Scopes)
                .HasColumnName("scopes")
                .HasColumnType("text");

            builder.Property(x => x.IsRevoked)
                .HasColumnName("is_revoked")
                .HasDefaultValue(true);

            builder.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasIndex(x => x.SessionId);

            builder.HasOne(x => x.Session)
                .WithMany(s => s.AccessTokens)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
