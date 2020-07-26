using System.Net.Http;

namespace Telegram.Bot.Args
{
    /// <summary>
    /// Provides data for MakingApiRequest event
    /// </summary>
    public class ApiRequestEventArgs
    {
        /// <summary>
        /// Bot API method name
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// HTTP content of the request message
        /// </summary>
        public HttpContent? HttpContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseEventArgs"/> class.
        /// </summary>
        /// <param name="methodName">Bot API method name</param>
        /// <param name="httpContent">HTTP content of the request message</param>
        public ApiRequestEventArgs(string methodName, HttpContent? httpContent)
        {
            MethodName = methodName;
            HttpContent = httpContent;
        }
    }
}
