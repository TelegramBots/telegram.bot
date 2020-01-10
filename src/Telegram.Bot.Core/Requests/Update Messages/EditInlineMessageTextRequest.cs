using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit text and game messages sent via the bot (for inline bots). On success True is returned.
    /// </summary>
    public class EditInlineMessageTextRequest : RequestBase<bool>,
                                                IFormattableMessage,
                                                IInlineMessage,
                                                IInlineReplyMarkupMessage
    {
        /// <inheritdoc />
        public string InlineMessageId { get; }

        /// <summary>
        /// New text of the message
        /// </summary>
        public string Text { get; }

        /// <inheritdoc />
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Disables link previews for links in this message
        /// </summary>
        public bool DisableWebPagePreview { get; set; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inlineMessageId and new text
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="text">New text of the message</param>
        public EditInlineMessageTextRequest(string inlineMessageId, string text)
            : base("editMessageText")
        {
            InlineMessageId = inlineMessageId;
            Text = text;
        }
    }
}
