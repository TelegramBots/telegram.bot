using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Stop updating a live location message sent by the bot
    /// </summary>
    public class StopMessageLiveLocationRequest : ChatIdRequestBase<Message>,
                                                  IInlineReplyMarkupMessage
    {
        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        public int MessageId { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        public StopMessageLiveLocationRequest([DisallowNull] ChatId chatId, int messageId)
            : base("stopMessageLiveLocation")
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
