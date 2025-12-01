using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("is_success")]
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Indicates whether the operation failed
        /// </summary>
        [JsonPropertyName("is_failure")]
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// The value returned on success (only available when IsSuccess is true)
        /// </summary>
        [JsonPropertyName("value")]
        public T? Value { get; private set; }

        /// <summary>
        /// The error information (only available when IsFailure is true)
        /// </summary>
        [JsonPropertyName("error")]
        public Error? Error { get; private set; }

        /// <summary>
        /// Trace identifier for request correlation
        /// </summary>
        [JsonPropertyName("trace_id")]
        public string? TraceId { get; private set; }

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
    }
}
