using ESS.Application.Common.Interface;
using ESS.Application.Properties.Get;
using ESS.Application.Units.List;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Properties.Create
{
    public sealed class Handler : IRequestHandler<Command, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(IPropertyRepository propertyRepository, IAppUserRepository appUserRepository, ICurrentUser currentUser)
        {
            _propertyRepository = propertyRepository;
            _appUserRepository = appUserRepository;
            _currentUser = currentUser;
        }

        public async Task<PropertyDto> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException();

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                throw new InvalidOperationException("current user not linked to an owner");


            var created = await _propertyRepository.CreateAsync(
                appUser.OwnerId.Value,
                command.Name,
                command.Address,
                command.City,
                command.Country,
                command.Description,
                ct);


            return new PropertyDto(
                created!.Id,
                created.Name,
                created.Address,
                created.City,
                created.Country,
                created.Description,
                OccupiedUnits: 0,
                TotalUnits: 0,
                Units: new List<UnitListItemDto>(),
                RentalOptions: new List<Rentals.Get.RentalOptionDto>()
                );
        }
    }
}
