using AuthService.Domain.Audit;
using AuthService.Domain.Grants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class AuthEventsConfiguration : IEntityTypeConfiguration<AuthEvents>
    {
        public void Configure(EntityTypeBuilder<AuthEvents> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.EventType)
                .HasColumnName("event_type")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(x => x.Severity)
                .HasColumnName("severity")
                .HasConversion<short>()
                .IsRequired();

            builder.Property(x => x.UserId)
               .HasColumnName("user_id");

            builder.Property(x => x.ClientId)
               .HasColumnName("client_id");

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255);

            builder.Property(x => x.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType("inet");

            builder.Property(x => x.UserAgent)
                .HasColumnName("user_agent")
                .HasMaxLength(500);

            builder.Property(x => x.Success)
                .HasColumnName("success")
                .IsRequired();

            builder.Property(x => x.ErrorCode)
                .HasColumnName("error_code")
                .HasMaxLength(255);

            builder.Property(x => x.ErrorMessage)
                .HasColumnName("error_message")
                .HasMaxLength(1000);

            builder.Property(x => x.MetadataJson)
                .HasColumnName("metadata_json")
                .HasColumnType("jsonb");

            builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("now()");

            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.IpAddress);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.EventType);
        }
    }
}
