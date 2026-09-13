using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Units.List
{
    public sealed record UnitListItemDto
    (
        Guid Id,
        string Name,
        string Description
    );
}
