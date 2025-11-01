namespace Api.GRRInnovations.Memorix.Application.Wrappers
{
    /// <summary>
    /// Represents an error with a structured code and message
    /// </summary>
    public record Error(string Code, string Message)
    {
        /// <summary>
        /// Represents no error (success state)
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty);

        /// <summary>
        /// Represents a null value error
        /// </summary>
        public static readonly Error NullValue = new("Error.NullValue", "The result value is null.");

        // Domain Errors
        /// <summary>
        /// Creates a "Not Found" error for the specified entity
        /// </summary>
        public static Error NotFound(string entity) => new("NotFound", $"{entity} not found.");

        /// <summary>
        /// Creates an "Unauthorized" error
        /// </summary>
        public static Error Unauthorized() => new("Unauthorized", "You don't have permission.");

        /// <summary>
        /// Creates a "Validation" error with the specified message
        /// </summary>
        public static Error Validation(string message) => new("Validation", message);

        /// <summary>
        /// Creates a "Conflict" error (e.g., duplicate resource)
        /// </summary>
        public static Error Conflict(string message) => new("Conflict", message);

        /// <summary>
        /// Creates a "Bad Request" error with the specified message
        /// </summary>
        public static Error BadRequest(string message) => new("BadRequest", message);

        /// <summary>
        /// Creates a "Internal Server Error" with the specified message
        /// </summary>
        public static Error InternalError(string message) => new("InternalError", message);
    }
}

