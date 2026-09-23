using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Domain.Properties.Property>
    {
        public void Configure(EntityTypeBuilder<Domain.Properties.Property> builder)
        {
            builder.ToTable("properties");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Address)
                .HasColumnName("address")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Country)
                .HasColumnName("country")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(255);

            builder.Property(x => x.IsEnabled)
                .HasColumnName("is_enabled")
                .HasDefaultValue(true);

            builder.Property(x => x.SoftDeletedAt)
                .HasColumnName("soft_deleted_at");

            builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

            builder.Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

            builder.HasOne<Domain.Owners.Owner>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
