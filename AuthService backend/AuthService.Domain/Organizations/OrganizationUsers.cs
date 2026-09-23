using AuthService.Domain.Users;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace AuthService.Domain.Organizations
{
    public class OrganizationUsers
    {
        private OrganizationUsers() { }

        public Guid Id { get; private set; }
        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public MembershipStatus Status { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Organization Organization { get; set; } = null!;
        public User User { get; set; } = null!;

        public static OrganizationUsers Create(Guid userId, Guid orgId, DateTime now)
        {
            return new OrganizationUsers
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrganizationId = orgId,
                Status = MembershipStatus.Active,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public void Activate(DateTime now)
        {
            if(Status == MembershipStatus.Active)
                return;

            Status = MembershipStatus.Active;
            UpdatedAt = now;
        }

        public enum MembershipStatus
        {
            Pending = 0,
            Active = 1,
            Disabled = 2
        }


    }


}
