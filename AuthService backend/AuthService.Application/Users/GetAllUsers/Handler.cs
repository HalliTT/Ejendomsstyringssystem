using AuthService.Application.Auth.Login;
using AuthService.Contracts.Users;
using AuthService.Domain.Common;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace AuthService.Application.Users.GetAllUsers
{
    public class Handler : IRequestHandler<Query, Result<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public Handler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<UserDto>>> Handle(Query request, CancellationToken ct)
        {
            var users = await _userRepository.GetAllAsync(ct);
            return Result<List<UserDto>>.Success(_mapper.Map<List<UserDto>>(users));
        }
    }
}
