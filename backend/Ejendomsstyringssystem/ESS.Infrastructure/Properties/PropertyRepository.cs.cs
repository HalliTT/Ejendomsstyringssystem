using ESS.Application.Properties;
using ESS.Application.Properties.List;
using ESS.Domain.Properties;
using ESS.Domain.Units;
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

        public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<Property?> CreateAsync(string name, string address, string city, string country, string description, CancellationToken ct)
        {
            var property = new Property { Name = name, Address = address, City = city, Country = country, Description = description };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync(ct);

            return property;
        }
    }
}
