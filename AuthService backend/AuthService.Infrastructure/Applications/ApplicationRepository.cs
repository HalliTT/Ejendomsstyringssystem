using AuthService.Application.Applications;
using AuthService.Application.Common;
using AuthService.Domain.Applications;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Applications
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AuthDbContext _db;
        public ApplicationRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<ApplicationResult?> GetByClientIdAsync(Guid clientId, CancellationToken ct)
        {
            return await _db.Applications
               .AsNoTracking()
               .Where(a =>
                   a.ClientId == clientId &&
                   a.IsEnabled == true &&
                   a.SoftDeletedAt == null)
               .Include(a => a.DeletedByUser)
               .Select(a => new ApplicationResult(
                   a.Id,
                   a.ClientId,
                   a.Name,
                   a.IsEnabled,
                   a.OrganizationId,
                   a.SoftDeletedAt,
                   a.ApplicationRedirectUrises.Select(aru => aru.RedirectUris ?? string.Empty).ToList(),
                   null,
                   null
               ))
               .SingleOrDefaultAsync(ct);
        }

        public async Task<IReadOnlyList<ApplicationResult>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct)
        {
            return await _db.Applications
                .AsNoTracking()
                .Where(a => a.OrganizationId == organizationId)
                .GroupJoin(
                    _db.Users,
                    app => app.DeletedBy,
                    usr => usr.Id,
                    (app, users) => new { App = app, User = users.FirstOrDefault() }
                )
                .OrderByDescending(x => x.App.CreatedAt)
                .Select(x => new ApplicationResult(
                    x.App.Id,
                    x.App.ClientId,
                    x.App.Name,
                    x.App.IsEnabled,
                    x.App.OrganizationId,
                    x.App.SoftDeletedAt,
                    x.App.ApplicationRedirectUrises.Select(aru => aru.RedirectUris ?? string.Empty).ToList(),
                    null,
                    x.User != null
                        ? new AuthService.Application.Common.UserMinimalResult(
                            x.User.Id,
                            x.User.FirstName,
                            x.User.LastName,
                            x.User.DisplayName)
                        : null
                ))
                .ToListAsync(ct);
        }

        public async Task<ApplicationResult> CreateAsync(Guid organizationId, string name, IReadOnlyList<string> redirectUris, DateTime now, CancellationToken ct)
        {
            var appId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var clientSecret = Guid.NewGuid().ToString("N");

            var application = new Domain.Applications.Application
            {
                Id = appId,
                OrganizationId = organizationId,
                Name = name,
                ClientId = clientId,
                ClientSecret = clientSecret,
                IsEnabled = true,
                CreatedAt = now,
                UpdatedAt = now
            };

            foreach (var uri in redirectUris)
            {
                application.ApplicationRedirectUrises.Add(new ApplicationRedirectUris
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = appId,
                    RedirectUris = uri,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            _db.Applications.Add(application);
            await _db.SaveChangesAsync(ct);

            return new ApplicationResult(
                application.Id,
                application.ClientId,
                application.Name,
                application.IsEnabled,
                application.OrganizationId,
                application.SoftDeletedAt,
                application.ApplicationRedirectUrises
                    .Select(x => x.RedirectUris ?? string.Empty)
                    .ToList(),
                clientSecret,
                null);
        }

        public async Task<ApplicationResult?> UpdateAsync(Guid organizationId, Guid applicationId, string name, bool isEnabled, DateTime now, CancellationToken ct)
        {
            var application = await _db.Applications
                .Include(x => x.ApplicationRedirectUrises)
                .SingleOrDefaultAsync(
                    x => x.OrganizationId == organizationId && x.Id == applicationId && x.SoftDeletedAt == null,
                    ct);

            if (application is null)
            {
                return null;
            }

            application.Name = name;
            application.IsEnabled = isEnabled;
            application.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);

            return new ApplicationResult(
                application.Id,
                application.ClientId,
                application.Name,
                application.IsEnabled,
                application.OrganizationId,
                application.SoftDeletedAt,
                application.ApplicationRedirectUrises.Select(x => x.RedirectUris ?? string.Empty).ToList(),
                null,
                null);
        }

        public async Task<bool> SoftDeleteAsync(Guid organizationId, Guid applicationId, Guid deletedBy, DateTime now, CancellationToken ct)
        {
            var application = await _db.Applications
                .SingleOrDefaultAsync(
                    x => x.OrganizationId == organizationId && x.Id == applicationId && x.SoftDeletedAt == null,
                    ct);

            if (application is null)
            {
                return false;
            }

            application.SoftDeletedAt = now;
            application.DeletedBy = deletedBy;
            application.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IReadOnlyList<ApplicationRedirectUriResult>> ListRedirectUrisAsync(Guid applicationId, CancellationToken ct)
        {
            return await _db.Set<ApplicationRedirectUris>()
                .AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new ApplicationRedirectUriResult(
                    x.Id,
                    x.ApplicationId,
                    x.RedirectUris ?? string.Empty,
                    x.CreatedAt,
                    x.UpdatedAt
                ))
                .ToListAsync(ct);
        }

        public async Task<bool> AddRedirectUriAsync(Guid applicationId, string redirectUri, DateTime now, CancellationToken ct)
        {
            var appExists = await _db.Applications
                .AsNoTracking()
                .AnyAsync(x => x.Id == applicationId && x.SoftDeletedAt == null, ct);

            if (!appExists)
            {
                return false;
            }

            var normalized = NormalizeRedirectUri(redirectUri);

            var duplicate = await _db.Set<ApplicationRedirectUris>()
                .AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .AnyAsync(x => NormalizeRedirectUri(x.RedirectUris ?? string.Empty) == normalized, ct);

            if (duplicate)
            {
                return false;
            }

            _db.Set<ApplicationRedirectUris>().Add(new ApplicationRedirectUris
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicationId,
                RedirectUris = redirectUri,
                CreatedAt = now,
                UpdatedAt = now
            });

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteRedirectUriAsync(Guid applicationId, Guid redirectUriId, CancellationToken ct)
        {
            var redirect = await _db.Set<ApplicationRedirectUris>()
                .SingleOrDefaultAsync(x => x.ApplicationId == applicationId && x.Id == redirectUriId, ct);

            if (redirect is null)
            {
                return false;
            }

            _db.Set<ApplicationRedirectUris>().Remove(redirect);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        private static string NormalizeRedirectUri(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var parsed))
            {
                return uri.Trim();
            }

            var port = parsed.IsDefaultPort ? string.Empty : $":{parsed.Port}";
            var path = string.IsNullOrEmpty(parsed.AbsolutePath) ? "/" : parsed.AbsolutePath.TrimEnd('/');
            return $"{parsed.Scheme}://{parsed.Host}{port}{path}";
        }
    }
}
