using AuthService.Domain.Addresses;
using AuthService.Domain.Grants;
using AuthService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Domain.Applications
{
    public class Application
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public bool IsEnabled { get; set; } = false;
        public DateTime? SoftDeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ApplicationRedirectUris> ApplicationRedirectUrises { get; set; } = new List<ApplicationRedirectUris>();
        public ICollection<Grant> Grants { get; private set; } = new List<Grant>();
        public User? DeletedByUser { get; set; }
    }
}
