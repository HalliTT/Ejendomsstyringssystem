using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Infrastructure.Persistence.Configuration
{
    public class RentalOptionUnitConfiguration : IEntityTypeConfiguration<Domain.Rentals.RentalOptionUnit>
    {
        public void Configure(EntityTypeBuilder<Domain.Rentals.RentalOptionUnit> builder)
        {

            builder.ToTable("rental_option_units");

            builder.HasKey(x => new
            {
                x.RentalOptionId,
                x.UnitId
            });

            builder.Property(x => x.RentalOptionId)
                .HasColumnName("rental_options_id");

            builder.Property(x => x.UnitId)
                .HasColumnName("unit_id");

            builder.HasOne<Domain.Rentals.RentalOption>()
                .WithMany()
                .HasForeignKey(x => x.RentalOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Domain.Units.Unit>()
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
