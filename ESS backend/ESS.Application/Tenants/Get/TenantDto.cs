namespace ESS.Application.Tenants.Get
{
    public sealed record TenantDto
    (
        Guid Id,
        string Name,
        string Email,
        string Phone
    );
}
