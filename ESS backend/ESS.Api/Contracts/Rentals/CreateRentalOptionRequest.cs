using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Rentals
{
    public sealed record CreateRentalOptionRequest
    (
        [Required]
        [MaxLength(255)]
        string Name,

        [Range(0, double.MaxValue)]
        decimal MonthlyRent,

        [Required]
        [MaxLength(50)]
        string Status,

        [Required]
        Guid PropertyId,

        List<Guid>? UnitIds
    );
}
