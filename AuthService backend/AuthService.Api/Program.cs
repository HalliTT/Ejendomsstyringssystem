using AuthService.Application;
using AuthService.Application.Auth.Password;
using AuthService.Application.Users;
using AuthService.Api.Middleware;
using AuthService.Api.Security;
using AuthService.Domain.Applications;
using AuthService.Domain.Organizations;
using AuthService.Domain.Users;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Persistence;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddAuthentication(OpaqueBearerAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, OpaqueBearerAuthenticationHandler>(
        OpaqueBearerAuthenticationDefaults.AuthenticationScheme,
        _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ScopePolicies.ManagementAccess, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new ManagementAccessRequirement());
    });
    options.AddPolicy(ScopePolicies.OrganizationRead, policy => policy.RequireClaim("scope", "organization.read"));
    options.AddPolicy(ScopePolicies.OrganizationWrite, policy => policy.RequireClaim("scope", "organization.write"));
    options.AddPolicy(ScopePolicies.ApplicationRead, policy => policy.RequireClaim("scope", "application.read"));
    options.AddPolicy(ScopePolicies.ApplicationWrite, policy => policy.RequireClaim("scope", "application.write"));
    options.AddPolicy(ScopePolicies.RedirectUriRead, policy => policy.RequireClaim("scope", "redirect_uri.read"));
    options.AddPolicy(ScopePolicies.RedirectUriWrite, policy => policy.RequireClaim("scope", "redirect_uri.write"));
});
builder.Services.AddScoped<IAuthorizationHandler, ManagementAccessRequirementHandler>();

// Mapster
var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
typeAdapterConfig.Scan(typeof(MappingConfig).Assembly);
builder.Services.AddSingleton(typeAdapterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Config CORS
string[] allowedOrigins =
[
    "http://localhost:3000",
    "http://localhost:3001",
    "http://localhost:3002",
    .. builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? []
];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        db.Database.Migrate();

        var predefinedUserId = Guid.Parse("aebf22f2-b076-493c-bda0-e27ac102187d");
        if (!db.Users.Any(u => u.Id == predefinedUserId))
        {
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();
            var hashed = passwordHasher.Hash("Password123!");

            var user = new User
            {
                Id = predefinedUserId,
                Email = "demo@IfPurhus.dk",
                PasswordHash = hashed,
                DisplayName = "Seed User",
                IsVerified = true,
                IsEnabled = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        var orgSlug = "local-demo-org";
        var org = db.Organizations
                    .AsNoTracking()
                    .FirstOrDefault(o => o.Slug == orgSlug);


        Guid organizationId;
        if (org is null)
        {
            organizationId = Guid.NewGuid();
            var organization = new Organization
            {
                Id = organizationId,
                Name = "Purhus IF",
                Description = "Organization for local demo app",
                Slug = orgSlug,
                OwnerId = predefinedUserId,
                CreatedBy = predefinedUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Organizations.Add(organization);

            // Link user as organization member
            var orgUser = OrganizationUsers.Create(predefinedUserId, organizationId, DateTime.UtcNow);
            db.OrganizationUsers.Add(orgUser);

            db.SaveChanges();
        }
        else
        {
            organizationId = org.Id;
        }

        var predefinedClientId = Guid.Parse("aebf55f2-b076-493c-bda0-e27ac102187d");
        var appId = db.Applications
                      .Where(a => a.ClientId == predefinedClientId)
                      .Select(a => (Guid?)a.Id)
                      .FirstOrDefault();

        if (appId is null)
        {
            appId = Guid.NewGuid();
            var clientSecret = Guid.NewGuid().ToString("N");

            var application = new Application
            {
                Id = appId.Value,
                Name = "Local Demo App",
                OrganizationId = organizationId,
                ClientId = predefinedClientId,
                ClientSecret = clientSecret,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Applications.Add(application);
            db.SaveChanges();
        }

        string[] predefinedRedirectUris =
        [
            "http://localhost:3001/callback",
            "http://8craft.dk/demo/callback"
        ];

        var redirectUris = db.Set<ApplicationRedirectUris>();
        foreach (var uri in predefinedRedirectUris)
        {
            if (!redirectUris.Any(r => r.ApplicationId == appId && r.RedirectUris == uri))
            {
                redirectUris.Add(new ApplicationRedirectUris
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = appId.Value,
                    RedirectUris = uri,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        db.SaveChanges();
    }
}

app.MapControllers();
app.Run();

public partial class Program
{
}
