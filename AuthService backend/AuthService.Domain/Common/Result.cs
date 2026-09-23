using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public static Result Success() => new(true);

        public static Result Failure() => new(false);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, T? value) : base(isSuccess)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
            => new(true, value);

        public static new Result<T> Failure()
            => new(false, default);
    }

    public class Result<T, TError> : Result
    {
        public T? Value { get; }
        public TError? Error { get; }

        private Result(bool isSuccess, T? value, TError? error)
            : base(isSuccess)
        {
            Value = value;
            Error = error;
        }

        public static Result<T, TError> Success(T value)
            => new(true, value, default);

        public static Result<T, TError> Failure(TError error)
            => new(false, default, error);
    }
}
