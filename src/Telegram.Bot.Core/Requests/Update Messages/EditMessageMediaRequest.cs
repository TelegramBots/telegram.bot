using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit audio, document, photo, or video messages. On success the edited <see cref="Message"/> is returned.
    /// </summary>
    public class EditMessageMediaRequest : FileRequestBase<Message>,
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
        /// New media content of the message
        /// </summary>
        public InputMediaBase Media { get; }

        /// <inheritdoc cref="IInlineReplyMarkupMessage.ReplyMarkup" />
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new media
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="media">New media content of the message</param>
        public EditMessageMediaRequest(ChatId chatId, int messageId, InputMediaBase media)
            : base("editMessageMedia")
        {
            ChatId = chatId;
            MessageId = messageId;
            Media = media;
        }

        // ToDo: If there is no file stream in the request, request content should be string
        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            var httpContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken);
            httpContent.AddContentIfInputFileStream(Media);
            return httpContent;
        }
    }
}
