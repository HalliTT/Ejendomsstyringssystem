using System;

namespace ESS.Domain.Rentals
{
    public class RentalOption
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal MonthlyRent { get; set; }
        public RentalOptionStatus Status { get; set; } = RentalOptionStatus.Available;

        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum RentalOptionStatus
    {
        Available,
        Unavailable
    }
}
