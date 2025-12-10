namespace CodeBase.Services.ToolBox.Services.Web
{
    /// <summary>
    /// Represents the status of a server response for a request.
    /// </summary>
    public enum ServerResponseCode
    {
        /// <summary>
        /// The request was successfully processed by the server.
        /// </summary>
        Success = 200,

        /// <summary>
        /// The request was received but there was a client-side error (e.g., bad request, validation failed).
        /// </summary>
        ClientError = 400,

        /// <summary>
        /// The request was received but there was a server-side error.
        /// </summary>
        ServerError = 500,

        /// <summary>
        /// The request timed out or could not connect to the server.
        /// </summary>
        Timeout = 408,

        /// <summary>
        /// The request was unauthorized (e.g., invalid token).
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// The request is pending and has not received a response yet.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// An unknown error occurred that doesn’t fall into other categories.
        /// </summary>
        Unknown = -1
    }
}