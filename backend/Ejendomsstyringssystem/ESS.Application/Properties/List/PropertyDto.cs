using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Properties.List
{
    public sealed record PropertyDto
    (
        Guid Id,
        string Name,
        string Address
    );
}
