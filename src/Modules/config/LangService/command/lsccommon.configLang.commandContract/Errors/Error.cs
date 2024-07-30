using lsccommon.configLang.commandContract.Abstractions;
using lsccommon.configLang.commandContract.Enumerations;

namespace lsccommon.configLang.commandContract.Errors
{
    /// <summary>
    /// Provide error for domain, contain error type and messages
    /// </summary>
    /// <param name="type">Type of error</param>
    /// <param name="messages">Error messages to provide more information</param>
    public class Error(ErrorType type, params string[]? messages) : IError
    {
        public IReadOnlyList<string>? Messages { get; } = messages;
        public ErrorType Type { get; } = type;
        public static readonly IError None = new Error(ErrorType.None, string.Empty);
        public static readonly IError NullValue = new Error(ErrorType.NullValue, "Null value was provided");

        /// <summary>
        /// Create not found error
        /// </summary>
        /// <param name="messages">Messages for not found error if any</param>
        /// <returns>Not found error</returns>
        public static IError NotFound(params string[]? messages)
        {
            return new Error(ErrorType.NotFound, messages);
        }

        /// <summary>
        /// Create internal server error
        /// </summary>
        /// <param name="messages">Messages for internal server error if any</param>
        /// <returns>Internal server error</returns>
        public static IError ServerError(params string[]? messages)
        {
            return new Error(ErrorType.ServerError, messages);
        }

        /// <summary>
        /// Create conflict error
        /// </summary>
        /// <param name="messages">Messages for conflict error if any</param>
        /// <returns>Conflict error</returns>
        public static IError Conflict(params string[]? messages)
        {
            return new Error(ErrorType.Conflict, messages);
        }

        /// <summary>
        /// Create validation problem error
        /// </summary>
        /// <param name="messages">Messages for validation problem error if any</param>
        /// <returns>Validation problem error</returns>
        public static IError ValidationProblem(params string[]? messages)
        {
            return new Error(ErrorType.ValidationProblem, messages);
        }
    }
}