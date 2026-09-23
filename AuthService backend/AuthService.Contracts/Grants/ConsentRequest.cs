using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Grants
{
    public sealed record ConsentRequest
    (
        Guid GrantId,
        bool Approved
    );
}
