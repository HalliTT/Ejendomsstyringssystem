
namespace ESS.Domain.Owners
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? SoftDeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
