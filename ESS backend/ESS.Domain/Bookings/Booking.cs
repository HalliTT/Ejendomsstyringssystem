using System;

namespace ESS.Domain.Bookings
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid RentalOptionId { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
