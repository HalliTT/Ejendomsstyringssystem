using AuthService.Application.Applications;
using AuthService.Application.Audit;
using AuthService.Application.Auth.Grants;
using AuthService.Application.Auth.Password;
using AuthService.Application.Auth.Session;
using AuthService.Application.Auth.Session.Tokens;
using AuthService.Application.Auth.Verify;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Application.Services;
using AuthService.Application.Users;
using AuthService.Infrastructure.Applications;
using AuthService.Infrastructure.Audit;
using AuthService.Infrastructure.Common;
using AuthService.Infrastructure.Grants;
using AuthService.Infrastructure.Organizations;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Security;
using AuthService.Infrastructure.Session;
using AuthService.Infrastructure.Tokens;
using AuthService.Infrastructure.Users;
using AuthService.Infrastructure.Verify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("NewDockerSetup")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IGrantRepository, GrantRepository>();
            services.AddScoped<IVerifyRepository, VerifyRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddScoped<IAccessRepository, AccessRepository>();
            services.AddScoped<IRefreshRepository, RefreshRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IMembershipService, MembershipService>();

            services.AddScoped<IAuditService, AuditServiceRepository>();
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IRequestContext, AuditHttpContext>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IClock, SystemClock>();
            services.AddScoped<IAuthorizationRequestStore, CookieAuthorizationRequestStore>();
            services.AddScoped<IFingerprintBuilder, FingerprintBuilder>();

            return services;
        }
    }
}
