using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Domain.Rentals
{
    public class RentalOption
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
