using System.Net.Http;

namespace Telegram.Bot.Args
{
    /// <summary>
    /// Provides data for <see cref="ITelegramBotClient.ApiResponseReceived"/> event
    /// </summary>
    public class ApiResponseEventArgs
    {
        /// <summary>
        /// HTTP response received from API
        /// </summary>
        public HttpResponseMessage ResponseMessage { get; }

        /// <summary>
        /// Event arguments of this request
        /// </summary>
        public ApiRequestEventArgs ApiRequestEventArgs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseEventArgs"/> class.
        /// </summary>
        /// <param name="httpResponseMessage">
        /// HTTP response received from API
        /// </param>
        /// <param name="apiRequestEventArgs">
        /// Event arguments of this request
        /// </param>
        public ApiResponseEventArgs(
            HttpResponseMessage httpResponseMessage,
            ApiRequestEventArgs apiRequestEventArgs)
        {
            ResponseMessage = httpResponseMessage;
            ApiRequestEventArgs = apiRequestEventArgs;
        }
    }
}
