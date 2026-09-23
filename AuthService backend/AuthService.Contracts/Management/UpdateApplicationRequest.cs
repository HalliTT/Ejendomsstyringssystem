namespace AuthService.Contracts.Management
{
    public sealed record UpdateApplicationRequest
    (
        string Name,
        bool IsEnabled
    );
}
