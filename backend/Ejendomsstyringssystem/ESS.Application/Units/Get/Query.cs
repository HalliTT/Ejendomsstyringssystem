using ESS.Application.Properties.Get;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Units.Get
{
    public sealed record Query(Guid UnitId) : IRequest<UnitDto?>;
}
