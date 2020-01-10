using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set a new profile photo for the chat. Photos can't be changed for private chats. The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    public sealed class SetChatPhotoRequest : ChatIdFileRequestBase<bool>
    {
        /// <summary>
        /// New chat photo
        /// </summary>
        [NotNull]
        public InputFileStream Photo { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and photo
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="photo">New chat photo</param>
        public SetChatPhotoRequest([NotNull] ChatId chatId, [NotNull] InputFileStream photo)
            : base("setChatPhoto")
        {
            ChatId = chatId;
            Photo = photo;
        }

        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            Photo.FileType == FileType.Stream
                ? await ToMultipartFormDataContentAsync(jsonConverter, "photo", Photo, cancellationToken)
                : await base.ToHttpContentAsync(jsonConverter, cancellationToken);
    }
}
