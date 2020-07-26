using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit live location messages sent by the bot
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EditMessageLiveLocationRequest : RequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; }

        /// <summary>
        /// Latitude of new location
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Latitude { get; }

        /// <summary>
        /// Longitude of new location
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Longitude { get; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and new location
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="latitude">Latitude of new location</param>
        /// <param name="longitude">Longitude of new location</param>
        public EditMessageLiveLocationRequest(
            ChatId chatId,
            int messageId,
            double latitude,
            double longitude)
            : base("editMessageLiveLocation")
        {
            ChatId = chatId;
            MessageId = messageId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
