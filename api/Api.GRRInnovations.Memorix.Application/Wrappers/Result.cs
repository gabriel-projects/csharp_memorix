using System;

namespace Api.GRRInnovations.Memorix.Application.Wrappers
{
    /// <summary>
    /// Represents the result of an operation that can succeed or fail
    /// </summary>
    /// <typeparam name="T">The type of the value returned on success</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Indicates whether the operation failed
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// The value returned on success (only available when IsSuccess is true)
        /// </summary>
        public T? Value { get; private set; }

        /// <summary>
        /// The error information (only available when IsFailure is true)
        /// </summary>
        public Error? Error { get; private set; }

        /// <summary>
        /// Trace identifier for request correlation
        /// </summary>
        public string? TraceId { get; private set; }

        // Backward compatibility properties
        /// <summary>
        /// Legacy property for backward compatibility (maps to IsSuccess)
        /// </summary>
        public bool Success => IsSuccess;

        /// <summary>
        /// Legacy property for backward compatibility (maps to Value)
        /// Note: This is readonly. Use Success() or Fail() static methods to create results.
        /// </summary>
        public T? Data => Value;

        /// <summary>
        /// Legacy property for backward compatibility (maps to Error.Message or null)
        /// </summary>
        public string? Message => Error?.Message;

        private Result(T value, string? traceId = null)
        {
            IsSuccess = true;
            Value = value;
            TraceId = traceId;
            Error = null;
        }

        private Result(Error error, string? traceId = null)
        {
            IsSuccess = false;
            Error = error ?? Error.None;
            TraceId = traceId;
            Value = default;
        }

        /// <summary>
        /// Creates a successful result with the specified value
        /// </summary>
        public static Result<T> SuccessResult(T value, string? traceId = null) => new(value, traceId);

        /// <summary>
        /// Creates a failed result with the specified error
        /// </summary>
        public static Result<T> Failure(Error error, string? traceId = null) => new(error, traceId);

        // Legacy static methods for backward compatibility
        /// <summary>
        /// Creates a successful result (legacy method for backward compatibility)
        /// </summary>
        public static Result<T> Ok(T data, string? message = null, string? traceId = null)
            => new(data, traceId);

        /// <summary>
        /// Creates a failed result (legacy method for backward compatibility)
        /// </summary>
        public static Result<T> Fail(string message, string? traceId = null)
            => new(new Error("Error", message), traceId);

        /// <summary>
        /// Creates a failed result with an Error object
        /// </summary>
        public static Result<T> Fail(Error error, string? traceId = null)
            => new(error, traceId);
    }
}
