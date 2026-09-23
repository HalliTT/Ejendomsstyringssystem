using AuthService.Contracts;
using AuthService.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AuthService.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private static readonly HashSet<string> SafeOAuthErrorCodes = new(StringComparer.OrdinalIgnoreCase)
        {
            "invalid_request",
            "invalid_client",
            "invalid_grant",
            "unauthorized_client",
            "server_error",
            "temporarily_unavailable"
        };

        protected IActionResult Ok<T>(Result<T> result, string? errorMessage = null)
        {
            if (result.IsSuccess)
            {
                return base.Ok(ApiResponse<T>.SuccessResponse(result.Value));
            }

            return BadRequest(ApiResponse<T>.ErrorResponse(
                errorMessage ?? "Operation failed"
            ));
        }

        protected IActionResult Ok<T, TError>(Result<T, TError> result)
        {
            if (result.IsSuccess)
            {
                return base.Ok(ApiResponse<T>.SuccessResponse(result.Value));
            }

            var (statusCode, safeErrorCode) = ResolveError(result.Error);
            return StatusCode(statusCode, ApiResponse<T>.ErrorResponse(safeErrorCode));
        }

        protected IActionResult Ok(Result result, string? message = null)
        {
            if (result.IsSuccess)
            {
                return base.Ok(ApiResponse.SuccessResponse(message ?? "Operation completed successfully"));
            }

            return BadRequest(ApiResponse.ErrorResponse(
                message ?? "Operation failed"
            ));
        }

        protected IActionResult SuccessResponse(string? message = null)
            => base.Ok(ApiResponse.SuccessResponse(message ?? "Operation completed successfully"));

        protected IActionResult UnauthorizedResponse(string errorCode = "invalid_client", string? message = null)
            => StatusCode(401, ApiResponse.ErrorResponse(ResolveSafeErrorMessage(errorCode), message ?? "Unauthorized"));

        protected IActionResult ForbiddenResponse(string errorCode = "unauthorized_client", string? message = null)
            => StatusCode(403, ApiResponse.ErrorResponse(ResolveSafeErrorMessage(errorCode), message ?? "Forbidden"));

        protected IActionResult NotFoundResponse(string errorCode = "invalid_request", string? message = null)
            => StatusCode(404, ApiResponse.ErrorResponse(ResolveSafeErrorMessage(errorCode), message ?? "Not found"));

        private static (int StatusCode, string SafeErrorCode) ResolveError<TError>(TError? error)
        {
            var safeError = ResolveSafeErrorMessage(error);
            var statusCode = safeError switch
            {
                "invalid_request" => 400,
                "invalid_client" => 401,
                "invalid_grant" => 400,
                "unauthorized_client" => 403,
                "server_error" => 500,
                "temporarily_unavailable" => 503,
                _ => 500
            };

            return (statusCode, safeError);
        }

        private static string ResolveSafeErrorMessage<TError>(TError? error)
        {
            if (error is string code)
            {
                var normalized = code.Trim();
                if (SafeOAuthErrorCodes.Contains(normalized))
                {
                    return normalized;
                }
            }

            return "server_error";
        }
    }
}
