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
    /// Send audio files, if you want Telegram clients to display the file as a playable voice message.
    /// For this to work, your audio must be in an .ogg file encoded with OPUS
    /// (other formats may be sent as Audio or Document).
    /// Bots can currently send voice messages of up to 50 MB in size, this limit may be changed in the future.
    /// </summary>
    public sealed class SendVoiceRequest : FileRequestBase<Message>,
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
        /// Audio file to send
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public InputOnlineFile Voice { get; }

        /// <summary>
        /// Duration of the voice message in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <summary>
        /// Voice message caption, 0-1024 characters
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
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel.</param>
        /// <param name="voice">Voice to send.</param>
        public SendVoiceRequest([DisallowNull] ChatId chatId, [DisallowNull] InputOnlineFile voice)
            : base("sendVoice")
        {
            ChatId = chatId;
            Voice = voice;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            Voice.FileType == FileType.Stream
                ? await ToMultipartFormDataContentAsync(jsonConverter, "voice", Voice, cancellationToken)
                : await base.ToHttpContentAsync(jsonConverter, cancellationToken);
    }
}
