using AuthService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Common
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
