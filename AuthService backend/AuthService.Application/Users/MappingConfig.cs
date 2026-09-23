using AuthService.Contracts.Addresses;
using AuthService.Contracts.Users;
using AuthService.Domain.Addresses;
using AuthService.Domain.Users;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Users
{
    public sealed class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserDto>();
            config.NewConfig<User, UserWithAddressDto>();
            config.NewConfig<Address, AddressDto>();
        }
    }
}
