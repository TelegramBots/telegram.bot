using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable CheckNamespace

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Stop a poll
    /// </summary>
    public class StopPollRequest : RequestBase<Poll>, IReplyMarkupMessage<InlineKeyboardMarkup>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the original message with the poll
        /// </summary>
        public int MessageId { get; }

        /// <inheritdoc />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new text
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="jsonConverter"></param>
        public StopPollRequest(ChatId chatId, int messageId)
            : base("stopPoll")
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
