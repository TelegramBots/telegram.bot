using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send a game. On success, the sent <see cref="Message"/> is returned.
    /// </summary>
    public class SendGameRequest : RequestBase<Message>,
                                   INotifiableMessage,
                                   IReplyMessage,
                                   IInlineReplyMarkupMessage
    {
        /// <summary>
        /// Unique identifier for the target chat
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Short name of the game, serves as the unique identifier for the game
        /// </summary>
        public string GameShortName { get; }

        /// <inheritdoc />
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and gameShortName
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat</param>
        /// <param name="gameShortName">Short name of the game, serves as the unique identifier for the game</param>
        public SendGameRequest(long chatId, string gameShortName)
            : base("sendGame")
        {
            ChatId = chatId;
            GameShortName = gameShortName;
        }
    }
}
