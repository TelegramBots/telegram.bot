using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Specify a url and receive incoming updates via an outgoing webhook
    /// </summary>
    public class SetWebhookRequest : FileRequestBase<bool>
    {
        /// <summary>
        /// HTTPS url to send updates to. Use an empty string to remove webhook integration.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Public key certificate so that the root certificate in use can be checked
        /// </summary>
        public InputFileStream Certificate { get; }

        /// <summary>
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// List the types of updates you want your bot to receive
        /// </summary>
        public IEnumerable<UpdateType> AllowedUpdates { get; set; }

        /// <summary>
        /// Initializes a new request with uri
        /// </summary>
        /// <param name="url">HTTPS url to send updates to</param>
        /// <param name="certificate">ToDo</param>
        public SetWebhookRequest(string url, InputFileStream certificate)
            : base("setWebhook")
        {
            Url = url;
            Certificate = certificate;
        }

        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc cref="RequestBase{TResponse}.ToHttpContentAsync"/>
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            Certificate == null
                ? await base.ToHttpContentAsync(jsonConverter, cancellationToken)
                : await ToMultipartFormDataContentAsync(jsonConverter, "certificate", Certificate, cancellationToken);
    }
}
