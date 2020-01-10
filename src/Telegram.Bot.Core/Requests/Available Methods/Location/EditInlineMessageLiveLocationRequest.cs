using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit live location messages sent via the bot (for inline bots)
    /// </summary>
    public class EditInlineMessageLiveLocationRequest : RequestBase<bool>,
                                                        IInlineMessage,
                                                        IInlineReplyMarkupMessage
    {
        /// <inheritdoc />
        public string InlineMessageId { get; }

        /// <summary>
        /// Latitude of new location
        /// </summary>
        public float Latitude { get; }

        /// <summary>
        /// Longitude of new location
        /// </summary>
        public float Longitude { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inline message id and new location
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="latitude">Latitude of new location</param>
        /// <param name="longitude">Longitude of new location</param>
        public EditInlineMessageLiveLocationRequest([NotNull] string inlineMessageId, float latitude, float longitude)
            : base("editMessageLiveLocation")
        {
            InlineMessageId = inlineMessageId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
