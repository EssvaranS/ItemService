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
        /// Creates a successful response.
        /// </summary>
        public static ApiResponse<T> Ok(T data, string message = "") => new() { Success = true, Message = message, Data = data };

        /// <summary>
        /// Creates a failed response.
        /// </summary>
        public static ApiResponse<T> Fail(string message) => new() { Success = false, Message = message, Data = default };
        }   
    }
