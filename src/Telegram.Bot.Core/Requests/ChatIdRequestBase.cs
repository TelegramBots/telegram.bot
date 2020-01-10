using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

namespace Telegram.Bot.Requests
{
    public abstract class ChatIdRequestBase<TResponse> : RequestBase<TResponse>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [NotNull]
        [DataMember(IsRequired = true)]
        public ChatId ChatId { get; set; }

        protected ChatIdRequestBase([NotNull] string methodName) : base(methodName)
        {
        }

        protected ChatIdRequestBase([NotNull] string methodName, [NotNull] HttpMethod method) : base(methodName, method)
        {
        }
    }
}
