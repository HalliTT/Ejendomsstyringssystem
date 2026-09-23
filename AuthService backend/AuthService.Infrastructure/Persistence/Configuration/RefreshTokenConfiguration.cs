using AuthService.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.TokenHash)
                .HasColumnName("token")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.Property(x => x.ReplacedByTokenId)
                .HasColumnName("replaced_by_token_id");

            builder.Property(x => x.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(x => x.Version)
                .IsRowVersion();

            builder.HasIndex(x => x.TokenHash).IsUnique();
            builder.HasIndex(x => x.SessionId);

            builder.HasOne(x => x.Session)
                .WithMany(s => s.RefreshTokens)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rt => rt.ReplacedByToken)
                .WithOne()
                .HasForeignKey<RefreshToken>(x => x.ReplacedByTokenId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
