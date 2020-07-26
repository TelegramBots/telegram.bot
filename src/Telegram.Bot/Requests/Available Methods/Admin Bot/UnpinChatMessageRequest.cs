using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Unpin a message in a supergroup or a channel. The bot must be an administrator in the chat
    /// for this to work and must have the <see cref="ChatMember.CanPinMessages"/> admin right in
    /// the supergroup or <see cref="ChatMember.CanEditMessages"/> admin right in the channel.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UnpinChatMessageRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; set; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        public UnpinChatMessageRequest(ChatId chatId)
            : base("unpinChatMessage")
        {
            ChatId = chatId;
        }
    }
}
