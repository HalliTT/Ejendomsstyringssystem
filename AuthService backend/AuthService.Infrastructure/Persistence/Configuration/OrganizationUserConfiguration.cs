using AuthService.Domain.Organizations;
using AuthService.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class OrganizationUserConfiguration : IEntityTypeConfiguration<OrganizationUsers>
    {
        public void Configure(EntityTypeBuilder<OrganizationUsers> builder)
        {
            builder.ToTable("organization_users");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.OrganizationId)
                .HasColumnName("organization_id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<short>()
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasOne(ou => ou.Organization)
                .WithMany(o => o.OrganizationUsers)
                .HasForeignKey(ou => ou.OrganizationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ou => ou.User)
                .WithMany(u => u.OrganizationUsers)
                .HasForeignKey(ou => ou.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
