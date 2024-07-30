using lsccommon.configLang.queryContract.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lsccommon.configLang.queryAPI.Middleware
{
    /// <summary>
    /// Handler for handling exception
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";
        private readonly IWebHostEnvironment env;

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        /// <summary>
        /// Will handle all exception and return problem as http response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task with a boolean result indicating if the exception was handled</returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext context,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            // Set the response content type
            var contentType = "application/problem+json";
            // Add an error code to the exception
            exception.AddErrorCode(ExceptionCodeConstant.UNEXPECTED);
            // Log the exception
            logger.LogError(exception, exception.Message);
            // Create problem details object
            var problemDetails = CreateProblemDetails(context, exception);
            // Serialize problem details to JSON
            var json = ToJson(problemDetails);
            // Write JSON response
            context.Response.ContentType = contentType;
            await context.Response.WriteAsync(json, cancellationToken);
            return true;
        }

        /// <summary>
        /// Create problem detail base on exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns>A ProblemDetails object containing information about the exception</returns>
        private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
        {
            // Get the response status code
            var statusCode = context.Response.StatusCode;
            // Get the reason phrase for the status code
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
            if (string.IsNullOrEmpty(reasonPhrase))
            {
                // Use a default message
                reasonPhrase = UnhandledExceptionMsg;
            }

            // Get the error code from the exception
            var errorCode = exception.GetErrorCode();
            // Create the ProblemDetails object
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = reasonPhrase,
                Extensions =
                {
                    [nameof(errorCode)] = errorCode
                }
            };
            if (env.IsProduction())
            {
                return problemDetails;
            }

            // Add detailed error information for non-production environments
            problemDetails.Detail = exception.ToString();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            // problemDetails.Extensions["data"] = exception.Data;
            return problemDetails;
        }

        /// <summary>
        /// Convert problem details to json
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <returns>A JSON string representation of the ProblemDetails object, or an empty string if serialization fails</returns>
        private string ToJson(in ProblemDetails problemDetails)
        {
            try
            {
                // Serialize the ProblemDetails object to JSON
                return JsonSerializer.Serialize(problemDetails, SerializerOptions);
            }
            catch (Exception ex)
            {
                // Log an error
                var msg = "An exception has occurred while serializing error to JSON";
                logger.LogError(ex, msg);
            }

            return string.Empty;
        }
    }
}