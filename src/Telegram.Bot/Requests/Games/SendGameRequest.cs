using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send a game. On success, the sent <see cref="Message"/> is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendGameRequest : RequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long ChatId { get; }

        /// <summary>
        /// Short name of the game, serves as the unique identifier for the game
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string GameShortName { get; }

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        /// <summary>
        /// If the message is a reply, ID of the original message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        /// <summary>
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        [JsonIgnore]
        ChatId IChatTargetable.ChatId => ChatId;

        /// <summary>
        /// Initializes a new request with chatId and gameShortName
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat</param>
        /// <param name="gameShortName">
        /// Short name of the game, serves as the unique identifier for the game
        /// </param>
        public SendGameRequest(long chatId, string gameShortName)
            : base("sendGame")
        {
            ChatId = chatId;
            GameShortName = gameShortName;
        }
    }
}
