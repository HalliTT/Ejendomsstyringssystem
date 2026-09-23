using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Common.Interface
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
    }
}
