using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Stop updating a live location message sent via the bot (for inline bots) before live
    /// period expires
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class StopInlineMessageLiveLocationRequest : RequestBase<bool>
    {
        /// <summary>
        /// Identifier of the inline message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineMessageId { get; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inlineMessageId
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        public StopInlineMessageLiveLocationRequest(string inlineMessageId)
            : base("stopMessageLiveLocation")
        {
            InlineMessageId = inlineMessageId;
        }
    }
}
