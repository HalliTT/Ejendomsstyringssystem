using AuthService.Contracts.Users;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Users.GetUserById
{
    public class Handler : IRequestHandler<Query, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(Query request, CancellationToken ct)
        {
            var users = await _userRepository.GetUserAsync(ct);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
