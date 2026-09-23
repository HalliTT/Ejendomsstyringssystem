using System;
using System.Collections.Generic;

namespace AuthService.Contracts
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Request processed successfully",
                Errors = null
            };
        }

        public static ApiResponse<T> ErrorResponse(string error, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message ?? "Request failed",
                Errors = new[] { error }
            };
        }

        public static ApiResponse<T> ErrorResponse(IEnumerable<string> errors, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message ?? "Request failed",
                Errors = errors
            };
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static ApiResponse SuccessResponse(string? message = null)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message ?? "Request processed successfully",
                Errors = null
            };
        }

        public static ApiResponse ErrorResponse(string error, string? message = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message ?? "Request failed",
                Errors = new[] { error }
            };
        }

        public static ApiResponse ErrorResponse(IEnumerable<string> errors, string? message = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message ?? "Request failed",
                Errors = errors
            };
        }
    }
}
