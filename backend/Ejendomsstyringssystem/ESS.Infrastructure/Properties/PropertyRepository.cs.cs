using ESS.Application.Properties;
using ESS.Domain.Properties;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Infrastructure.Properties
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly EssDbContext _context;

        public PropertyRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Property>> ListAsync(CancellationToken ct)
        {
            return await _context.Properties
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
