namespace ItemService.Api.Responses
{
        /// <summary>
        /// Standardized API response wrapper for consistent output.
        /// </summary>
        /// <typeparam name="T">Type of the response data.</typeparam>
        public class ApiResponse<T>
        {
            /// <summary>
            /// Indicates if the request was successful.
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Message describing the result or error.
            /// </summary>
            public string Message { get; set; } = string.Empty;

            /// <summary>
            /// Response data payload.
            /// </summary>
            public T? Data { get; set; }

            /// <summary>
            /// HTTP status code for the response.
            /// </summary>
            public int StatusCode { get; set; }

            /// <summary>
            /// Creates a successful response.
            /// </summary>
            public static ApiResponse<T> Ok(T data, string message = "", int statusCode = 200) => new() { Success = true, Message = message, Data = data, StatusCode = statusCode };

            /// <summary>
            /// Creates a failed response.
            /// </summary>
            public static ApiResponse<T> Fail(string message, int statusCode = 400) => new() { Success = false, Message = message, Data = default, StatusCode = statusCode };
        }
    }
