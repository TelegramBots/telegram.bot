using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a text message to be sent as the result of an
    /// <see cref="InlineQuery"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputTextMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Text of the message to be sent, 1-4096 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MessageText { get; private set; }

        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic,
        /// fixed-width text or inline URLs in your bot's message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <summary>
        /// Optional. Disables link previews for links in the sent message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableWebPagePreview { get; set; }

#pragma warning disable 8618
        private InputTextMessageContent()
#pragma warning restore 8618
        { }

        /// <summary>
        /// Initializes a new input text message content
        /// </summary>
        /// <param name="messageText">The text of the message</param>
        public InputTextMessageContent(string messageText)
        {
            MessageText = messageText;
        }
    }
}
