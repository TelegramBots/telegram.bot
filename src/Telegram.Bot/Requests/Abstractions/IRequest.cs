using System.Net.Http;

// ReSharper disable once UnusedTypeParameter
namespace Telegram.Bot.Requests.Abstractions
{
    /// <summary>
    /// Represents a request to Bot API
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public interface IRequest<TResponse>
    {
        /// <summary>
        /// HTTP method of request
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// API method name
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        HttpContent? ToHttpContent();

        /// <summary>
        /// Allows this object to be used as a response in webhooks
        /// </summary>
        bool IsWebhookResponse { get; set; }
    }
}
