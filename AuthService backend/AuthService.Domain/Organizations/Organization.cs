using AuthService.Domain.Addresses;
using AuthService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Domain.Organizations
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; } = null!;
        public Guid? OwnerId { get; set; }
        public Guid? AddressId { get; set; }
        public DateTime? SoftDeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Address? Address { get; set; }
        public User? Owner { get; set; }
        public User? DeletedByUser { get; set; }
        public User? CreatedByUser { get; set; }

        public ICollection<OrganizationUsers> OrganizationUsers { get; set; } = new List<OrganizationUsers>();
    }
}
