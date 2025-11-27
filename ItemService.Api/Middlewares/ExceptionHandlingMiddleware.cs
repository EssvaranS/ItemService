using System.Net;
using System.Text.Json;

namespace ItemService.Api.Middlewares
{
    /// <summary>
    /// Middleware for global exception handling. Catches unhandled exceptions and returns a standardized error response.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">Logger for exception details.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware logic to catch and handle exceptions.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to next middleware
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";

                var (errorCode, statusCode, message) = ErrorCodeMapper.Map(ex);
                context.Response.StatusCode = statusCode;

                // Return standardized error response with error code
                var errorPayload = new
                {
                    success = false,
                    errorCode,
                    message,
                    details = ex.StackTrace
                };
                var json = JsonSerializer.Serialize(errorPayload);
                await context.Response.WriteAsync(json);
            }
        }
    }

    /// <summary>
    /// Error codes for standardized error responses.
    /// </summary>
    /// <summary>
    /// Error codes for standardized error responses in ItemService.
    /// </summary>
    public enum ErrorCodes : int
    {
        InternalServerError = 0,
        BadRequestError = 1,
        ResourceNotFoundError = 2,
        ResourceConflictError = 3,
    }
    /// <summary>
    /// Helper for mapping exceptions to error codes and status codes.
    /// </summary>
    public static class ErrorCodeMapper
    {
        public static (string errorCode, int statusCode, string message) Map(Exception ex)
        {
            ErrorCodes code;
            int status;
            string msg;
            switch (ex)
            {
                case ArgumentException:
                case FormatException:
                    code = ErrorCodes.BadRequestError;
                    status = (int)HttpStatusCode.BadRequest;
                    msg = ex.Message;
                    break;
                case KeyNotFoundException:
                    code = ErrorCodes.ResourceNotFoundError;
                    status = (int)HttpStatusCode.NotFound;
                    msg = ex.Message;
                    break;
                case InvalidOperationException:
                    code = ErrorCodes.ResourceConflictError;
                    status = (int)HttpStatusCode.Conflict;
                    msg = ex.Message;
                    break;
                default:
                    code = ErrorCodes.InternalServerError;
                    status = (int)HttpStatusCode.InternalServerError;
                    msg = "An unexpected error occurred.";
                    break;
            }
            // Use ErrorCodeExtensions to get custom error code string
            return (code.ToErrorCodeString(), status, msg);
        }
    }

    /// <summary>
    /// Extension methods for ErrorCodes.
    /// </summary>
    public static class ErrorCodeExtensions
    {
        private const string SERVICE_IDENTIFIER = "ITEMSVC"; // Service identifier

        public static string ToErrorCodeString(this ErrorCodes errorCode)
        {
            return $"{SERVICE_IDENTIFIER}-{(int)errorCode:X5}";
        }
    }
}
