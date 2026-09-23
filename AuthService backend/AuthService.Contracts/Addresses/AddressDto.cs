using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Addresses
{
    public class AddressDto
    {
        public Guid Id { get; init; }
        public string? AddressLineOne { get; init; }
        public string? AddressLineTwo { get; init; }
        public string? City { get; init; }
        public string? Zip { get; init; }
        public string? Country { get; init; }
    }
}
