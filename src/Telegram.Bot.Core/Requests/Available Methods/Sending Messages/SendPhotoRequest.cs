using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;
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
    /// Send photos
    /// </summary>
    public sealed class SendPhotoRequest : FileRequestBase<Message>,
                                           IChatMessage,
                                           INotifiableMessage,
                                           IReplyMessage,
                                           IReplyMarkupMessage<IReplyMarkup>,
                                           IFormattableMessage
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Photo to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Photo { get; }

        /// <summary>
        /// Photo caption (may also be used when resending photos by file_id), 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false), AllowNull, MaybeNull]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false), AllowNull, MaybeNull]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="photo">Photo to send</param>
        public SendPhotoRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile photo)
            : base("sendPhoto")
        {
            ChatId = chatId;
            Photo = photo;
        }

        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            Photo.FileType == FileType.Stream
                ? await ToMultipartFormDataContentAsync(jsonConverter, "photo", Photo, cancellationToken)
                : await base.ToHttpContentAsync(jsonConverter, cancellationToken);
    }
}
