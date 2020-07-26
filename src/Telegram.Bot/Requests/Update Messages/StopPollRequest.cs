using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable CheckNamespace

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Stop a poll
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class StopPollRequest : RequestBase<Poll>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the original message with the poll
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new text
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="messageId">Identifier of the sent message</param>
        public StopPollRequest(ChatId chatId, int messageId)
            : base("stopPoll")
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
