using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify
{
    public interface IAuthorizationRequestStore
    {
        void SetGrantIt(Guid grantId);
        Guid? GetGrantId();
        void Clear();
    }
}
