using AuthService.Application.Common;
using AuthService.Application.Organizations;
using AuthService.Domain.Organizations;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Organizations
{
    public sealed class OrganizationRepository : IOrganizationRepository
    {
        private readonly AuthDbContext _db;

        public OrganizationRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Organizations.SingleOrDefaultAsync(g => g.Id == id && g.SoftDeletedAt == null, ct);
        }

        public async Task<bool> IsUserInOrganizationAsync(Guid userId, Guid OrgId, CancellationToken ct)
        {
            return await _db.OrganizationUsers
                .AsNoTracking()
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.OrganizationId == OrgId &&
                    x.Organization.SoftDeletedAt == null,
                    ct);
        }

        public async Task<IReadOnlyList<OrganizationResult>> ListAsync(CancellationToken ct)
        {
            return await _db.Organizations
                .AsNoTracking()
                .GroupJoin(
                    _db.Users,
                    org => org.DeletedBy,
                    usr => usr.Id,
                    (org, users) => new { Org = org, User = users.FirstOrDefault() }
                )
                .OrderByDescending(x => x.Org.CreatedAt)
                .Select(x => new OrganizationResult(
                    x.Org.Id,
                    x.Org.Name ?? string.Empty,
                    x.Org.Slug,
                    x.Org.Description,
                    x.Org.OwnerId,
                    x.Org.CreatedAt,
                    x.Org.UpdatedAt,
                    x.Org.SoftDeletedAt,
                    x.User != null
                        ? new AuthService.Application.Common.UserMinimalResult(
                            x.User.Id,
                            x.User.FirstName,
                            x.User.LastName,
                            x.User.DisplayName)
                        : null))
                .ToListAsync(ct);
        }

        public async Task<OrganizationResult> CreateAsync(string name, string? description, string slug, Guid ownerId, Guid createdBy, DateTime now, CancellationToken ct)
        {
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Slug = slug,
                OwnerId = ownerId,
                CreatedBy = createdBy,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Organizations.Add(organization);
            await _db.SaveChangesAsync(ct);

            return new OrganizationResult(
                organization.Id,
                organization.Name ?? string.Empty,
                organization.Slug,
                organization.Description,
                organization.OwnerId,
                organization.CreatedAt,
                organization.UpdatedAt,
                organization.SoftDeletedAt,
                null);
        }

        public async Task<bool> ExistsActiveAsync(Guid organizationId, CancellationToken ct)
        {
            return await _db.Organizations
                .AsNoTracking()
                .AnyAsync(x => x.Id == organizationId && x.SoftDeletedAt == null, ct);
        }

        public async Task<OrganizationResult?> UpdateAsync(Guid organizationId, string name, string? description, string slug, Guid? ownerId, DateTime now, CancellationToken ct)
        {
            var organization = await _db.Organizations
                .SingleOrDefaultAsync(x => x.Id == organizationId && x.SoftDeletedAt == null, ct);

            if (organization is null)
            {
                return null;
            }

            organization.Name = name;
            organization.Description = description;
            organization.Slug = slug;
            organization.OwnerId = ownerId;
            organization.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);

            return new OrganizationResult(
                organization.Id,
                organization.Name ?? string.Empty,
                organization.Slug,
                organization.Description,
                organization.OwnerId,
                organization.CreatedAt,
                organization.UpdatedAt,
                organization.SoftDeletedAt,
                null);
        }

        public async Task<bool> SoftDeleteAsync(Guid organizationId, Guid deletedBy, DateTime now, CancellationToken ct)
        {
            var organization = await _db.Organizations
                .SingleOrDefaultAsync(x => x.Id == organizationId && x.SoftDeletedAt == null, ct);

            if (organization is null)
            {
                return false;
            }

            organization.SoftDeletedAt = now;
            organization.DeletedBy = deletedBy;
            organization.UpdatedAt = now;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
        {
            return await _db.Organizations
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && x.SoftDeletedAt == null, ct);
        }

        public async Task<bool> SlugExistsForOtherAsync(Guid organizationId, string slug, CancellationToken ct)
        {
            return await _db.Organizations
                .AsNoTracking()
                .AnyAsync(x => x.Id != organizationId && x.Slug == slug && x.SoftDeletedAt == null, ct);
        }
    }
}
