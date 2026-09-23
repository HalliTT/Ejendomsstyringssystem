using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Rentals
{
    public sealed record UpdateRentalOptionRequest
    (
        [Required]
        [MaxLength(255)]
        string Name,

        [Range(0, double.MaxValue)]
        decimal MonthlyRent,

        [Required]
        [MaxLength(50)]
        string Status,

        List<Guid>? UnitIds
    );
}
