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
    /// Send rounded square mp4 video messages of up to 1 minute long.
    /// </summary>
    public sealed class SendVideoNoteRequest : FileRequestBase<Message>,
                                               IChatMessage,
                                               INotifiableMessage,
                                               IReplyMessage,
                                               IReplyMarkupMessage<IReplyMarkup>,
                                               IThumbMediaMessage
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Video note to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputTelegramFile VideoNote { get; }

        /// <summary>
        /// Duration of sent video in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <summary>
        /// Video width and height, i.e. diameter of the video message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Length { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia? Thumb { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="videoNote">Video note to send</param>
        public SendVideoNoteRequest([DisallowNull] ChatId chatId, [DisallowNull] InputTelegramFile videoNote)
            : base("sendVideoNote")
        {
            ChatId = chatId;
            VideoNote = videoNote;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            if (VideoNote.FileType != FileType.Stream && Thumb?.FileType != FileType.Stream)
                return await base.ToHttpContentAsync(jsonConverter, cancellationToken);

            var multipartContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, "video_note", "thumb");

            if (VideoNote.FileType == FileType.Stream)
                multipartContent.AddStreamContent(VideoNote.Content, "video_note", VideoNote.FileName);
            if (Thumb?.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Thumb.Content, "thumb", Thumb.FileName);

            return multipartContent;
        }
    }
}
