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
    /// Add a new sticker to a set created by the bot. Returns True on success.
    /// </summary>
    public class AddStickerToSetRequest : FileRequestBase<bool>
    {
        /// <summary>
        /// User identifier of sticker set owner
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Sticker set name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must not exceed 512px, and either width or height must be exactly 512px
        /// </summary>
        public InputOnlineFile PngSticker { get; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        public string Emojis { get; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        public MaskPosition MaskPosition { get; set; }

        /// <summary>
        /// Initializes a new request with userId, name pngSticker and emojis
        /// </summary>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="pngSticker">Png image with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        public AddStickerToSetRequest(int userId, string name, InputOnlineFile pngSticker, string emojis)
            : base("addStickerToSet")
        {
            UserId = userId;
            Name = name;
            PngSticker = pngSticker;
            Emojis = emojis;
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
