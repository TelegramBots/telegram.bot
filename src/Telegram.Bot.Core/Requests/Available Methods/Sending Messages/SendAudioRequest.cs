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
    /// Send audio files, if you want Telegram clients to display them in the music player.
    /// Your audio must be in the .MP3 or .M4A format.
    /// Bots can currently send audio files of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    public sealed class SendAudioRequest : FileRequestBase<Message>,
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
        /// Audio file to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Audio { get; }

        /// <summary>
        /// Audio caption, 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Duration of the audio in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <summary>
        /// Performer
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Performer { get; set; }

        /// <summary>
        /// Track name
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Title { get; set; }

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
        /// <param name="audio">Audio to send</param>
        public SendAudioRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile audio)
            : base("sendAudio")
        {
            ChatId = chatId;
            Audio = audio;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            if (Audio.FileType != FileType.Stream && Thumb?.FileType != FileType.Stream)
                return await base.ToHttpContentAsync(jsonConverter, cancellationToken);

            var multipartContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, "audio", "thumb");

            if (Audio.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Audio.Content, "audio", Audio.FileName);
            if (Thumb?.FileType == FileType.Stream)
                multipartContent.AddStreamContent(Thumb.Content, "thumb", Thumb.FileName);

            return multipartContent;
        }
    }
}
