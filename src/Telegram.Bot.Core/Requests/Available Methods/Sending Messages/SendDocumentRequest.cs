using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send general files.
    /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    public sealed class SendDocumentRequest : FileRequestBase<Message>,
                                              IChatMessage,
                                              INotifiableMessage,
                                              IReplyMessage,
                                              IReplyMarkupMessage<IReplyMarkup>,
                                              IFormattableMessage,
                                              IThumbMediaMessage
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// File to send.
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Document { get; }

        /// <summary>
        /// Document caption (may also be used when resending documents by file_id), 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia? Thumb { get; set; }

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
        [DataMember(EmitDefaultValue = false)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="document">Document to send</param>
        public SendDocumentRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile document)
            : base("sendDocument")
        {
            ChatId = chatId;
            Document = document;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            if (Document.FileType != FileType.Stream && Thumb?.FileType != FileType.Stream)
                return await base.ToHttpContentAsync(jsonConverter, cancellationToken);

            var multipartContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, "document", "thumb");

            if (Document.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Document.Content, "document", Document.FileName);
            if (Thumb?.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Thumb.Content, "thumb", Thumb.FileName);

            return multipartContent;

        }
    }
}
