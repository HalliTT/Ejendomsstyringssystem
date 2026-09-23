using AuthService.Application.Auth.Verify;
using AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Security
{
    public sealed class CookieAuthorizationRequestStore : IAuthorizationRequestStore
    {
        private const string CookieName = "auth_request";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClock _clock;

        public CookieAuthorizationRequestStore(IHttpContextAccessor httpContextAccessor, IClock clock)
        {
            _httpContextAccessor = httpContextAccessor;
            _clock = clock;
        }

        public void SetGrantIt(Guid grantId)
        {
            var response = _httpContextAccessor.HttpContext?.Response;

            if (response == null)
            {
                throw new InvalidOperationException("HTTP Response is not available.");
            }

            response.Cookies.Append(
                CookieName,
                grantId.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = _clock.UtcNow.AddMinutes(10)
                });
        }

        public Guid? GetGrantId()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HTTP Request is not available.");

            if (!request.Cookies.TryGetValue(CookieName, out var value))
                return null;

            return Guid.TryParse(value, out var id) ? id : null;
        }

        public void Clear()
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(CookieName);
        }
    }
}
