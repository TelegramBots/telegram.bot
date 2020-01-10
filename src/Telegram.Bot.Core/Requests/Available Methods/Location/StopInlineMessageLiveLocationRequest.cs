using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Stop updating a live location message sent via the bot (for inline bots) before live period expires
    /// </summary>
    public class StopInlineMessageLiveLocationRequest : RequestBase<bool>,
                                                        IInlineMessage,
                                                        IInlineReplyMarkupMessage
    {
        /// <inheritdoc />
        [NotNullIfNotNull("inlineMessageId")]
        public string InlineMessageId { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with inlineMessageId
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        public StopInlineMessageLiveLocationRequest([DisallowNull] string inlineMessageId)
            : base("stopMessageLiveLocation")
        {
            InlineMessageId = inlineMessageId;
        }
    }
}
