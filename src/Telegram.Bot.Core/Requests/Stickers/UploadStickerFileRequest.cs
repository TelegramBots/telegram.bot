using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = Telegram.Bot.Types.File;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Upload a .png file with a sticker for later use in createNewStickerSet and addStickerToSet methods (can be used multiple times). Returns the uploaded <see cref="File"/> on success.
    /// </summary>
    public class UploadStickerFileRequest : FileRequestBase<File>
    {
        /// <summary>
        /// User identifier of sticker file owner
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must not exceed 512px, and either width or height must be exactly 512px.
        /// </summary>
        public InputFileStream PngSticker { get; }

        /// <summary>
        /// Initializes a new request with userId and pngSticker
        /// </summary>
        public UploadStickerFileRequest(int userId, InputFileStream pngSticker)
            : base("uploadStickerFile")
        {
            UserId = userId;
            PngSticker = pngSticker;
        }

        /// <param name="jsonConverter"></param>
        /// <param name="cancellationToken"></param>
        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken) =>
            PngSticker.FileType == FileType.Stream
                ? await ToMultipartFormDataContentAsync(jsonConverter, "png_sticker", PngSticker, cancellationToken)
                : await base.ToHttpContentAsync(jsonConverter, cancellationToken);
    }
}
