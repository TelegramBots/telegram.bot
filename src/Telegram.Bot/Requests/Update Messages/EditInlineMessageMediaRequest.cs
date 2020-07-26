using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit audio, document, photo, or video inline messages
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EditInlineMessageMediaRequest : RequestBase<bool>
    {
        /// <summary>
        /// Identifier of the inline message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineMessageId { get; }

        /// <summary>
        /// New media content of the message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputMediaBase Media { get; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inlineMessageId and new media
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="media">New media content of the message</param>
        public EditInlineMessageMediaRequest(string inlineMessageId, InputMediaBase media)
            : base("editMessageMedia")
        {
            InlineMessageId = inlineMessageId;
            Media = media;
        }
    }
}
