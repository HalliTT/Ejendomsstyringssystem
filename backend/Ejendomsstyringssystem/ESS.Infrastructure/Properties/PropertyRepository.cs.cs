using ESS.Application.Properties;
using ESS.Domain.Properties;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ESS.Infrastructure.Properties
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly EssDbContext _context;

        public PropertyRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Property>> ListAsync(Guid ownerId, CancellationToken ct)
        {
            return await _context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == ownerId && p.SoftDeletedAt == null)
                .ToListAsync(ct);
        }

        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.SoftDeletedAt == null, ct);
        }

        public async Task<Property?> CreateAsync(Guid ownerId, string name, string address, string city, string country, string description, CancellationToken ct)
        {
            var property = new Property { OwnerId = ownerId, Name = name, Address = address, City = city, Country = country, Description = description };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync(ct);

            return property;
        }

        public async Task<Property?> UpdateAsync(Guid id, string name, string address, string city, string country, string description, CancellationToken ct)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id && p.SoftDeletedAt == null, ct);
            if (property is null)
                return null;

            property.Name = name;
            property.Address = address;
            property.City = city;
            property.Country = country;
            property.Description = description;
            property.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return property;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id && p.SoftDeletedAt == null, ct);
            if (property is null)
                return false;

            property.SoftDeletedAt = DateTime.UtcNow;
            property.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
