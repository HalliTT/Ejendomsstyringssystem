using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Units
{
    public sealed record CreateUnitRequest
    (
        [MaxLength(255)]
        [Required]
        string Name,

        [MaxLength(255)]
        string Description,

        [Required]
        string Status,

        [Required]
        Guid PropertyId
    );
}
