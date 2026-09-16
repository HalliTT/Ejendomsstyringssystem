using ESS.Domain.Owners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<Domain.Users.AppUser>
    {
        public void Configure(EntityTypeBuilder<Domain.Users.AppUser> builder)
        {
            builder.ToTable("app_users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(255);

            builder.Property(x => x.OwnerId)
                .HasColumnName("owner_id");

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

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
