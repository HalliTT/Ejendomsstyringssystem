using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Common.Interfaces
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
