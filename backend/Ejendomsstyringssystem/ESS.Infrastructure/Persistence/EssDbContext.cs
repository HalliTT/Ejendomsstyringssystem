using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ESS.Domain.Properties;
using ESS.Domain.Owners;
using ESS.Domain.Units;
using ESS.Domain.Users;
using ESS.Domain.Rentals;

namespace ESS.Infrastructure.Persistence
{
    public class EssDbContext : DbContext
    {
        public EssDbContext(DbContextOptions<EssDbContext> options) : base(options) { }
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Unit> Unit => Set<Unit>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<RentalOption> RentalOptions => Set<RentalOption>();
        public DbSet<RentalOptionUnit> RentalOptionUnits => Set<RentalOptionUnit>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Unit>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EssDbContext).Assembly);
        }
    }
}
