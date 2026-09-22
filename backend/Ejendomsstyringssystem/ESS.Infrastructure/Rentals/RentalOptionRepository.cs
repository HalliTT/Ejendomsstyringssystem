using ESS.Application.Rentals;
using ESS.Domain.Rentals;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ESS.Infrastructure.Rentals
{
    public class RentalOptionRepository : IRentalOptionRepository
    {
        private readonly EssDbContext _context;

        public RentalOptionRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<RentalOption>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct)
        {
            return await _context.RentalOptions
                .AsNoTracking()
                .Where(r => r.PropertyId == propertyId && r.SoftDeletedAt == null)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyDictionary<Guid, List<Guid>>> GetUnitIdsByRentalOptionIdsAsync(IReadOnlyList<Guid> rentalOptionIds, CancellationToken ct)
        {
            var links = await _context.RentalOptionUnits
                .AsNoTracking()
                .Where(l => rentalOptionIds.Contains(l.RentalOptionId))
                .ToListAsync(ct);

            return links
                .GroupBy(l => l.RentalOptionId)
                .ToDictionary(g => g.Key, g => g.Select(l => l.UnitId).ToList());
        }

        public async Task<RentalOption?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.RentalOptions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.SoftDeletedAt == null, ct);
        }

        public async Task<RentalOption?> CreateAsync(string name, decimal monthlyRent, RentalOptionStatus status, Guid propertyId, IReadOnlyList<Guid> unitIds, CancellationToken ct)
        {
            var option = new RentalOption
            {
                Name = name,
                MonthlyRent = monthlyRent,
                Status = status,
                PropertyId = propertyId,
            };

            _context.RentalOptions.Add(option);
            await _context.SaveChangesAsync(ct);

            foreach (var unitId in unitIds.Distinct())
            {
                _context.RentalOptionUnits.Add(new RentalOptionUnit { RentalOptionId = option.Id, UnitId = unitId });
            }
            await _context.SaveChangesAsync(ct);

            return option;
        }

        public async Task<RentalOption?> UpdateAsync(Guid id, string name, decimal monthlyRent, RentalOptionStatus status, IReadOnlyList<Guid> unitIds, CancellationToken ct)
        {
            var option = await _context.RentalOptions.FirstOrDefaultAsync(r => r.Id == id && r.SoftDeletedAt == null, ct);
            if (option is null)
                return null;

            option.Name = name;
            option.MonthlyRent = monthlyRent;
            option.Status = status;
            option.UpdatedAt = DateTime.UtcNow;

            var existingLinks = await _context.RentalOptionUnits
                .Where(l => l.RentalOptionId == id)
                .ToListAsync(ct);
            _context.RentalOptionUnits.RemoveRange(existingLinks);

            foreach (var unitId in unitIds.Distinct())
            {
                _context.RentalOptionUnits.Add(new RentalOptionUnit { RentalOptionId = id, UnitId = unitId });
            }

            await _context.SaveChangesAsync(ct);
            return option;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var option = await _context.RentalOptions.FirstOrDefaultAsync(r => r.Id == id && r.SoftDeletedAt == null, ct);
            if (option is null)
                return false;

            option.SoftDeletedAt = DateTime.UtcNow;
            option.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
