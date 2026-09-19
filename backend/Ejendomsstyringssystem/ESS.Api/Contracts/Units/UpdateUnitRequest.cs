using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Units
{
    public sealed record UpdateUnitRequest
    (
        [Required]
        [MaxLength(255)]
        string Name,

        [MaxLength(255)]
        string Description,

        [Required]
        [MaxLength(255)]
        string Status
    );
}
