using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Domain.Bookings.Booking>
    {
        public void Configure(EntityTypeBuilder<Domain.Bookings.Booking> builder)
        {
            builder.ToTable("bookings");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.RentalOptionId)
                .HasColumnName("rental_option_id")
                .IsRequired();

            builder.Property(x => x.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .IsRequired();

            builder.HasOne<Domain.Rentals.RentalOption>()
                .WithMany()
                .HasForeignKey(x => x.RentalOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Domain.Tenants.Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
