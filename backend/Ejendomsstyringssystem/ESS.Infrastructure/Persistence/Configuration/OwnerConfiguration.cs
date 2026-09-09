using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Domain.Owners.Owner>
    {
        public void Configure(EntityTypeBuilder<Domain.Owners.Owner> builder)
        {
            builder.ToTable("owners");

            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            builder.Property(x => x.Email)
                .HasColumnName("email")
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
        }
    }
}
