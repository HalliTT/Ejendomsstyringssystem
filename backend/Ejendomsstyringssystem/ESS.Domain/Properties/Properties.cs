using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Domain.Properties
{
    public class Property
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }

        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
