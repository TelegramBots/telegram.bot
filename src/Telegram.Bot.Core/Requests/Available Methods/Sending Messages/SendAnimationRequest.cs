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
    /// Send animation files (GIF or H.264/MPEG-4 AVC video without sound).
    /// Bots can currently send animation files of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    public sealed class SendAnimationRequest : FileRequestBase<Message>,
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
        /// Animation to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Animation { get; }

        /// <summary>
        /// Duration of sent animation in seconds
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

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia? Thumb { get; set; }

        /// <summary>
        /// Animation caption (may also be used when resending animation by file_id), 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

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
        /// <param name="animation">Animation to send</param>
        public SendAnimationRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile animation)
            : base("sendAnimation")
        {
            ChatId = chatId;
            Animation = animation;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            if (Animation.FileType != FileType.Stream && Thumb?.FileType != FileType.Stream)
                return await base.ToHttpContentAsync(jsonConverter, cancellationToken);

            var multipartContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, "animation", "thumb");

            if (Animation.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Animation.Content, "animation", Animation.FileName);
            if (Thumb?.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Thumb.Content, "thumb", Thumb.FileName);

            return multipartContent;
        }
    }
}
