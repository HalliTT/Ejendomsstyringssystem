using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Properties
{
    public sealed record UpdatePropertyRequest
    (
        [Required]
        [MaxLength(255)]
        string Name,

        [MaxLength(255)]
        string Address,

        [MaxLength(255)]
        string City,

        [MaxLength(255)]
        string Country,

        [MaxLength(255)]
        string Description
    );
}
