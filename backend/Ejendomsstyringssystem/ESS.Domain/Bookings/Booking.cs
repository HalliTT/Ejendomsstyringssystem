using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Domain.Bookings
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid RentalOptionId { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
