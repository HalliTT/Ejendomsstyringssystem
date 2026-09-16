using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Domain.Tenants
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
