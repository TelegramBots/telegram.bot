using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="jsonConverter">JSON converter that is used for (de)serialization of the JSON</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Content of HTTP request</returns>
        ValueTask<HttpContent> ToHttpContentAsync(ITelegramBotJsonConverter jsonConverter, CancellationToken cancellationToken);
    }
}
