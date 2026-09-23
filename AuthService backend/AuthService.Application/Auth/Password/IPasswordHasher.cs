using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Password
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
