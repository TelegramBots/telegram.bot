using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit captions and game messages sent by the bot. On success the edited <see cref="Message"/> is returned.
    /// </summary>
    public class EditMessageCaptionRequest : RequestBase<Message>,
                                             IInlineReplyMarkupMessage,
                                             IFormattableMessage
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
        /// New caption of the message
        /// </summary>
        public string Caption { get; set; }

        /// <inheritdoc />
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new caption
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="caption">New caption of the message</param>
        public EditMessageCaptionRequest(ChatId chatId, int messageId,
                                         string caption = default)
            : base("editMessageCaption")
        {
            ChatId = chatId;
            MessageId = messageId;
            Caption = caption;
        }
    }
}
