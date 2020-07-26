using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit only the reply markup of messages sent via the bot. On success the edited <c>true</c>
    /// is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EditInlineMessageReplyMarkupRequest : RequestBase<bool>
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
        /// Initializes a new request with inlineMessageId and new inline keyboard
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard</param>
        public EditInlineMessageReplyMarkupRequest(
            string inlineMessageId,
            InlineKeyboardMarkup? replyMarkup = default)
            : base("editMessageReplyMarkup")
        {
            InlineMessageId = inlineMessageId;
            ReplyMarkup = replyMarkup;
        }
    }
}
