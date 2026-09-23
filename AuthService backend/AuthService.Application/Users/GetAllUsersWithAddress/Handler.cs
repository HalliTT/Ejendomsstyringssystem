using AuthService.Contracts.Users;
using AuthService.Domain.Users;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace AuthService.Application.Users.GetAllUsersWithAddress
{
    public class Handler : IRequestHandler<Query, List<UserWithAddressDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public Handler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserWithAddressDto>> Handle(Query request, CancellationToken ct)
        {
            var users = await _userRepository.GetAllWithAddressAsync(ct);
            return _mapper.Map<List<UserWithAddressDto>>(users);
        }
    }
}
