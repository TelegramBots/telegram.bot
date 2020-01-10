using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send .webp stickers. On success, the sent <see cref="Message"/> is returned.
    /// </summary>
    public class SendStickerRequest : FileRequestBase<Message>,
                                      INotifiableMessage,
                                      IReplyMessage,
                                      IReplyMarkupMessage<IReplyMarkup>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channelusername)
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Sticker to send
        /// </summary>
        public InputOnlineFile Sticker { get; }

        /// <inheritdoc />
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        public IReplyMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request chatId and sticker
        /// </summary>
        public SendStickerRequest(ChatId chatId, InputOnlineFile sticker)
            : base("sendSticker")
        {
            ChatId = chatId;
            Sticker = sticker;
        }

        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            Sticker.FileType == FileType.Stream
                ? await ToMultipartFormDataContentAsync(jsonConverter, "sticker", Sticker, cancellationToken)
                : await base.ToHttpContentAsync(jsonConverter, cancellationToken);
    }
}
