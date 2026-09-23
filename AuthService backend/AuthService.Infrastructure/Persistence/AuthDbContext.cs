using AuthService.Domain.Audit;
using AuthService.Domain.Grants;
using AuthService.Domain.Organizations;
using AuthService.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Domain.Applications.Application> Applications => Set<Domain.Applications.Application>();
        public DbSet<Grant> Grants => Set<Grant>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<OrganizationUsers> OrganizationUsers => Set<OrganizationUsers>();


        public DbSet<Domain.Tokens.AccessToken> AccessTokens => Set<Domain.Tokens.AccessToken>();
        public DbSet<Domain.Tokens.RefreshToken> RefreshTokens => Set<Domain.Tokens.RefreshToken>();
        public DbSet<Domain.Sessions.Session> Session => Set<Domain.Sessions.Session>();

        public DbSet<AuthEvents> AuthEvents => Set<AuthEvents>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
