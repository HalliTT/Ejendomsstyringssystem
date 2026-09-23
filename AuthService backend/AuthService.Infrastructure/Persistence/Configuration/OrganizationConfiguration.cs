using AuthService.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("organizations");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(255);

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(255);
            builder.HasIndex(x => x.Id)
                .IsUnique();

            builder.Property(x => x.OwnerId)
                .HasColumnName("owner_id");

            builder.Property(x => x.AddressId)
                .HasColumnName("address_id");

            builder.Property(x => x.SoftDeletedAt)
                .HasColumnName("soft_deleted_at");

            builder.Property(x => x.DeletedBy)
                .HasColumnName("deleted_by");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.HasOne(o => o.Address)
                .WithMany(a => a.Organizations)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.Owner)
                .WithMany(u => u.OwnedOrganizations)
                .HasForeignKey(o => o.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.DeletedByUser)
                .WithMany(u => u.DeletedOrganizations)
                .HasForeignKey(o => o.DeletedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.CreatedByUser)
                .WithMany(u => u.CreatedOrganizations)
                .HasForeignKey(o => o.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
