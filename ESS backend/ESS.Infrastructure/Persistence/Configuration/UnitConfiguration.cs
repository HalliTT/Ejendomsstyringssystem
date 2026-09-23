using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class UnitConfiguration : IEntityTypeConfiguration<Domain.Units.Unit>
    {
        public void Configure(EntityTypeBuilder<Domain.Units.Unit> builder)
        {
            builder.ToTable("units");


            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.PropertyId)
                .HasColumnName("property_id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasColumnName("status")
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

            builder.HasOne<Domain.Properties.Property>()
                .WithMany()
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

