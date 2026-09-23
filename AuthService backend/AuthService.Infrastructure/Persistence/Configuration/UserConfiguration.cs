using AuthService.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(255);

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(255);

            builder.Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(255);

            builder.Property(x => x.Avatar)
                .HasColumnName("avatar")
                .HasMaxLength(255);

            builder.Property(x => x.PasswordHash)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.IsVerified)
                .HasColumnName("is_verified")
                .HasDefaultValue(false);

            builder.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .HasDefaultValue(false);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(false);

            builder.Property(x => x.IsAdmin)
                .HasColumnName("is_admin")
                .HasDefaultValue(false);

            builder.Property(x => x.LastLoginAt)
                .HasColumnName("last_login_at");

            builder.Property(x => x.SoftDeletedAt)
                .HasColumnName("soft_deleted_at");

            builder.Property(x => x.AddressId)
                .HasColumnName("address_id");

            builder.Property(x => x.DeletedBy)
                .HasColumnName("deleted_by");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()");

            builder.HasOne(u => u.Address)
                .WithMany(a => a.Users)
                .HasForeignKey(u => u.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
