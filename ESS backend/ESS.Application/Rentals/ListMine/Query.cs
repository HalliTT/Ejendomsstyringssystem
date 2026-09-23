using MediatR;

namespace ESS.Application.Rentals.ListMine
{
    public sealed record Query : IRequest<IReadOnlyList<RentalOptionSummaryDto>>;
}
