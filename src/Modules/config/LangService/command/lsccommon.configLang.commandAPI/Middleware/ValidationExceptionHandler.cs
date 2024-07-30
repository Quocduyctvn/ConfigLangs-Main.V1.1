using lsccommon.configLang.commandContract.Errors;
using lsccommon.configLang.commandDomain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace lsccommon.configLang.commandAPI.Middleware
{
    /// <summary>
    /// Handler for handling validation exception
    /// </summary>
    public class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ValidationExceptionHandler> logger;

        public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Try to handle validation exception. If fail to handle, will pass to another pipeline
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task with a boolean result indicating if the exception was handled</returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                // Log the exception
                logger.LogError(exception, $"Exception occured: {exception.Message}");
                // Set the response status code to 400 Bad Request
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                // Write the validation problem details to the response as JSON
                await httpContext.Response.WriteAsJsonAsync(Error.ValidationProblem(validationException.ValidationResults.ToArray()), cancellationToken);
                return true;
            }

            return false;
        }
    }
}