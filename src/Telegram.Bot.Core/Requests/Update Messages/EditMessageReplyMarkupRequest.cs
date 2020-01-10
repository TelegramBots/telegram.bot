using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit only the reply markup of messages sent by the bot. On success the edited <see cref="Message"/> is returned.
    /// </summary>
    public class EditMessageReplyMarkupRequest : RequestBase<Message>,
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

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new inline keyboard
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="replyMarkup">New inline keyboard of the sent message</param>
        public EditMessageReplyMarkupRequest(
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup replyMarkup = default)
            : base("editMessageReplyMarkup")
        {
            ChatId = chatId;
            MessageId = messageId;
            ReplyMarkup = replyMarkup;
        }
    }
}
