using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Tell the user that something is happening on the bot's side
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendChatActionRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Type of action to broadcast
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatAction Action { get; }

        /// <summary>
        /// Initializes a new request chatId and action
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="action">Type of action to broadcast</param>
        public SendChatActionRequest(ChatId chatId, ChatAction action)
            : base("sendChatAction")
        {
            ChatId = chatId;
            Action = action;
        }
    }
}
