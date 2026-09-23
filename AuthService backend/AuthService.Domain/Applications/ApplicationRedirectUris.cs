using AuthService.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Applications
{
    public class ApplicationRedirectUris
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string? RedirectUris { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Application? Applications { get; set; }
    }
}
