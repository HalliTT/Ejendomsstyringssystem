namespace ESS.Api.Security
{
    public sealed class UserInfoResponse
    {
        public bool Success { get; set; }
        public UserInfoData? Data { get; set; }
    }

    public sealed class UserInfoData
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public bool IsAdmin { get; set; }
        public string? Avatar { get; set; }
        public List<string> Scopes { get; set; } = new();
    }
}
