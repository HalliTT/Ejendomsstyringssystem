using System;
using System.Collections.Generic;
using System.Text;
using ESS.Application.Properties.List;
using ESS.Domain.Properties;

namespace ESS.Application.Properties
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyList<Property>> ListAsync(CancellationToken ct);
    }
}
