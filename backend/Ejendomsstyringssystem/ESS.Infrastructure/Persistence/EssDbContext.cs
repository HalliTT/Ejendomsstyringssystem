using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ESS.Domain.Properties;
using ESS.Domain.Owners;

namespace ESS.Infrastructure.Persistence
{
    public class EssDbContext : DbContext
    {
        public EssDbContext(DbContextOptions<EssDbContext> options) : base(options) { }
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Owner> Owners => Set<Owner>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EssDbContext).Assembly);
        }
    }
}
