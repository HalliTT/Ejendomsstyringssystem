using ESS.Application.Units;
using ESS.Domain.Units;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ESS.Infrastructure.Units
{
    public class UnitsRepository : IUnitRepository
    {
        private readonly EssDbContext _context;

        public UnitsRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Unit>> ListAsync(CancellationToken ct)
        {
            return await _context.Unit
                .AsNoTracking()
                .Where(u => u.SoftDeletedAt == null)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Unit>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct)
        {
            return await _context.Unit
                .AsNoTracking()
                .Where(u => u.PropertyId == propertyId && u.SoftDeletedAt == null)
                .ToListAsync(ct);
        }

        public async Task<Unit?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Unit
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.SoftDeletedAt == null, ct);
        }

        public async Task<Unit?> CreateAsync(string name, string description, UnitStatus status, Guid propertyId ,CancellationToken ct)
        {
            var unit = new Unit { Name = name, Description = description, Status = status, PropertyId = propertyId };

            _context.Unit.Add(unit);
            await _context.SaveChangesAsync(ct);

            return unit;

        }

        public async Task<Unit?> UpdateAsync(Guid id, string name, string description, UnitStatus status, CancellationToken ct)
        {
            var unit = await _context.Unit.FirstOrDefaultAsync(u => u.Id == id && u.SoftDeletedAt == null, ct);
            if (unit is null)
                return null;

            unit.Name = name;
            unit.Description = description;
            unit.Status = status;
            unit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return unit;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var unit = await _context.Unit.FirstOrDefaultAsync(u => u.Id == id && u.SoftDeletedAt == null, ct);
            if (unit is null)
                return false;

            unit.SoftDeletedAt = DateTime.UtcNow;
            unit.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
