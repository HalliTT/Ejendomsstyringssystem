using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Grants.ConsentApplication
{
    public sealed record ConsentResult
    (
        string redirectUrl
    );
}
