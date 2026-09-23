using AuthService.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Organization
{
    public class OrganizationUserDto
    {
        public Guid Id { get; set; }
        public OrganizationDto? Organization { get; init; }
        public UserDto? User { get; init; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}
