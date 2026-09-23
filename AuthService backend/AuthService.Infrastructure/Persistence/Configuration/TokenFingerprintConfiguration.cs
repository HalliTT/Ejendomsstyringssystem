using AuthService.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class TokenFingerprintConfiguration : IEntityTypeConfiguration<TokenFingerprint>
    {
        public void Configure(EntityTypeBuilder<TokenFingerprint> builder)
        {
            builder.ToTable("token_fingerprints");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            builder.Property(x => x.DeviceFingerprint)
                .HasColumnName("device_fingerprint")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.IpRangeHash)
                .HasColumnName("ip_range_hash")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.UserAgentHash)
                .HasColumnName("user_agent_hash")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.SessionId);

            builder.HasOne(x => x.Session)
                .WithMany(s => s.Fingerprints)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
