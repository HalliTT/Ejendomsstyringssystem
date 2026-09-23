using AuthService.Domain.Audit;
using AuthService.Domain.Organizations;
using AuthService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text;

namespace AuthService.Domain.Addresses
{
    public class Address
    {
        public Guid Id { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Organization> Organizations { get; set; } = new List<Organization>();

    }
}
