namespace ESS.Domain.Units
{
    public class Unit
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UnitStatus Status { get; set; } = UnitStatus.Maintenance;
        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum UnitStatus
    {
        Available,
        Occupied,
        Maintenance
    }
}
