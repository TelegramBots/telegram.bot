using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Telegram.Bot.Types;

namespace Telegram.Bot.Requests
{
    public abstract class ChatIdFileRequestBase<TResponse> : FileRequestBase<TResponse>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [NotNull]
        public ChatId ChatId { get; set; }

        protected ChatIdFileRequestBase([NotNull] string methodName) : base(methodName)
        {
        }

        protected ChatIdFileRequestBase([NotNull] string methodName, [NotNull] HttpMethod method) : base(methodName, method)
        {
        }
    }
}
