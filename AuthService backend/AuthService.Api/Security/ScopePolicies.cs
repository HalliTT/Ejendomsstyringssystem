namespace AuthService.Api.Security
{
    public static class ScopePolicies
    {
        public const string ManagementAccess = "management:access";

        public const string OrganizationRead = "scope:organization.read";
        public const string OrganizationWrite = "scope:organization.write";

        public const string ApplicationRead = "scope:application.read";
        public const string ApplicationWrite = "scope:application.write";

        public const string RedirectUriRead = "scope:redirect_uri.read";
        public const string RedirectUriWrite = "scope:redirect_uri.write";
    }
}
