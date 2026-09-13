using ESS.Application.Units;
using ESS.Domain.Units;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Unit>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct)
        {
            return await _context.Unit
                .AsNoTracking()
                .Where(u => u.PropertyId == propertyId)
                .ToListAsync(ct);
        }
    }
}
