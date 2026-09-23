namespace AuthService.Domain.Audit
{
    public enum AuthEventTypes
    {
        LOGIN_SUCCESS,
        LOGIN_FAILURE,
        CONSENT_APPROVED,
        CONSENT_DENIED,
        TOKEN_ISSUED,
        TOKEN_REFRESHED,
        TOKEN_REFRESH_FAILED,
        TOKEN_REVOKED,
        INVALID_CLIENT,
        INVALID_GRANT,
        PKCE_VERIFICATION_FAILED,
        ACCOUNT_LOCKED,
        REFRESH_TOKEN_REUSE
    }
}
