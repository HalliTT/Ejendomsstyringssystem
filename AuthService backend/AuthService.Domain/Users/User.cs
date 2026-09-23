using AuthService.Domain.Addresses;
using AuthService.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AuthService.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }

        public string? Avatar {  get; set; }

        public string PasswordHash { get; set; } = null!;
        public bool IsVerified { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public DateTime? SoftDeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? AddressId { get; set; }

        public Address? Address { get; set; }
        public ICollection<Organization> OwnedOrganizations { get; set; } = new List<Organization>();
        public ICollection<Organization> CreatedOrganizations { get; set; } = new List<Organization>();
        public ICollection<Organization> DeletedOrganizations { get; set; } = new List<Organization>();
        public ICollection<OrganizationUsers> OrganizationUsers { get; set; } = new List<OrganizationUsers>();
    }
}
