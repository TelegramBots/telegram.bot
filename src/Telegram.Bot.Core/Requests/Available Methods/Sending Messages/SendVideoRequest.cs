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
    /// Send video files.
    /// Telegram clients support mp4 videos (other formats may be sent as Document).
    /// Bots can currently send video files of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    public sealed class SendVideoRequest : FileRequestBase<Message>,
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
        /// Video to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Video { get; }

        /// <summary>
        /// Duration of the video in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <summary>
        /// Video width
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Width { get; set; }

        /// <summary>
        /// Video height
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Height { get; set; }

        /// <summary>
        /// Video caption (may also be used when resending videos by file_id), 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Pass True, if the uploaded video is suitable for streaming
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool SupportsStreaming { get; set; }

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
        /// <param name="video">Video to send</param>
        public SendVideoRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile video)
            : base("sendVideo")
        {
            ChatId = chatId;
            Video = video;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            if (Video.FileType != FileType.Stream && Thumb?.FileType != FileType.Stream)
                return await base.ToHttpContentAsync(jsonConverter, cancellationToken);

            var multipartContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, "video", "thumb");

            if (Video.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Video.Content, "video", Video.FileName);
            if (Thumb?.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Thumb.Content, "thumb", Thumb.FileName);

            return multipartContent;
        }
    }
}
