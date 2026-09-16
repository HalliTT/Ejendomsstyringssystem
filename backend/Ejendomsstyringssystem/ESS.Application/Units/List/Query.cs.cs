using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Units.List
{
    public sealed record Query : IRequest<IReadOnlyList<UnitListItemDto>>;
}
