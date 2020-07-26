using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit live location messages sent via the bot (for inline bots)
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EditInlineMessageLiveLocationRequest : RequestBase<bool>
    {
        /// <summary>
        /// Identifier of the inline message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineMessageId { get; }

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
        /// Initializes a new request with inline message id and new location
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="latitude">Latitude of new location</param>
        /// <param name="longitude">Longitude of new location</param>
        public EditInlineMessageLiveLocationRequest(
            string inlineMessageId,
            double latitude,
            double longitude)
            : base("editMessageLiveLocation")
        {
            InlineMessageId = inlineMessageId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
