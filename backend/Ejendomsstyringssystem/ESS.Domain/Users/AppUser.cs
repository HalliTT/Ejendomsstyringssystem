using ESS.Domain.Owners;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Domain.Users
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public Guid? OwnerId { get; set; }
        public Owner? Owner { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
