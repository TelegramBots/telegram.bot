using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Represents a request that doesn't require any parameters
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ParameterlessRequest<TResult> : RequestBase<TResult>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ParameterlessRequest{TResult}"/>
        /// </summary>
        /// <param name="jsonConverter"></param>
        /// <param name="methodName">Name of request method</param>
        public ParameterlessRequest(string methodName)
            : base(methodName)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterlessRequest{TResult}"/>
        /// </summary>
        /// <param name="jsonConverter"></param>
        /// <param name="methodName">Name of request method</param>
        /// <param name="method">HTTP request method</param>
        public ParameterlessRequest(string methodName, HttpMethod method)
            : base(methodName, method)
        {
        }

        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc cref="RequestBase{TResponse}.ToHttpContentAsync"/>
        public override ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) => IsWebhookResponse
            ? base.ToHttpContentAsync(jsonConverter, cancellationToken)
            : default;
    }
}
