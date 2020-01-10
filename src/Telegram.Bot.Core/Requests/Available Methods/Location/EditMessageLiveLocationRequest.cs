using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit live location messages sent by the bot
    /// </summary>
    public class EditMessageLiveLocationRequest : ChatIdRequestBase<Message>,
                                                  IInlineReplyMarkupMessage
    {
        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        /// Latitude of new location
        /// </summary>
        public float Latitude { get; }

        /// <summary>
        /// Longitude of new location
        /// </summary>
        public float Longitude { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with <see cref="ChatId"/> and new location
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="latitude">Latitude of new location</param>
        /// <param name="longitude">Longitude of new location</param>
        public EditMessageLiveLocationRequest([DisallowNull] ChatId chatId, int messageId, float latitude, float longitude)
            : base("editMessageLiveLocation")
        {
            ChatId = chatId;
            MessageId = messageId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
