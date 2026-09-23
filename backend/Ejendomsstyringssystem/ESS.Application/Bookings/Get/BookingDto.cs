namespace ESS.Application.Bookings.Get
{
    public sealed record BookingDto
    (
        Guid Id,
        Guid RentalOptionId,
        string RentalOptionName,
        Guid PropertyId,
        string PropertyName,
        Guid TenantId,
        string TenantName,
        string TenantEmail,
        string TenantPhone,
        DateTime StartDate,
        DateTime EndDate,
        BookingStatus Status
    );
}
