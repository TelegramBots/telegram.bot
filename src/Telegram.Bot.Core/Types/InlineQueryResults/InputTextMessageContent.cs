using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a text message to be sent as the result of an <see cref="InlineQuery"/>.
    /// </summary>
    [DataContract]
    public class InputTextMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Text of the message to be sent, 1-4096 characters
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MessageText { get; private set; }

        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Optional. Disables link previews for links in the sent message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool DisableWebPagePreview { get; set; }

        private InputTextMessageContent()
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
