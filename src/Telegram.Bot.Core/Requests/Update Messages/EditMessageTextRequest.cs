using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit text and game messages sent by the bot. On success the edited <see cref="Message"/> is returned.
    /// </summary>
    public class EditMessageTextRequest : RequestBase<Message>,
                                          IFormattableMessage,
                                          IInlineReplyMarkupMessage
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channelusername)
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        public int MessageId { get; }

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
        /// Initializes a new request with chatId, messageId and new text
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="text">New text of the message</param>
        public EditMessageTextRequest(ChatId chatId, int messageId, string text)
            : base("editMessageText")
        {
            ChatId = chatId;
            MessageId = messageId;
            Text = text;
        }
    }
}
