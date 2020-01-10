using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit audio, document, photo, or video inline messages
    /// </summary>
    public class EditInlineMessageMediaRequest : RequestBase<bool>,
        IInlineMessage,
        IInlineReplyMarkupMessage
    {
        /// <inheritdoc />
        public string InlineMessageId { get; }

        /// <summary>
        /// New media content of the message
        /// </summary>
        public InputMediaBase Media { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inlineMessageId and new media
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="media">New media content of the message</param>
        public EditInlineMessageMediaRequest(string inlineMessageId, InputMediaBase media)
            : base("editMessageMedia")
        {
            InlineMessageId = inlineMessageId;
            Media = media;
        }
    }
}
