using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configuration
{
    public class SessionConfiguration : IEntityTypeConfiguration<AuthService.Domain.Sessions.Session>
    {
        public void Configure(EntityTypeBuilder<AuthService.Domain.Sessions.Session> builder)
        {
            builder.ToTable("sessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.IsRevoked)
                .HasColumnName("is_revoked")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(x => x.LastActivity)
                .HasColumnName("last_activity");

            builder.Property(x => x.Version)
                .IsRowVersion();

            builder.HasIndex(x => x.UserId);
        }
    }
}
