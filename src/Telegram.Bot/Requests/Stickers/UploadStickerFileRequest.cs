using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = Telegram.Bot.Types.File;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Upload a .png file with a sticker for later use in <see cref="CreateNewStickerSetRequest"/>
    /// and <see cref="AddStickerToSetRequest"/> requests (can be used multiple times).
    /// Returns the uploaded <see cref="File"/> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UploadStickerFileRequest : FileRequestBase<File>, IUserTargetable
    {
        /// <summary>
        /// User identifier of sticker file owner
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must not
        /// exceed 512px, and either width or height must be exactly 512px.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
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

        /// <inheritdoc />
        public override HttpContent? ToHttpContent() =>
            PngSticker.FileType == FileType.Stream
                ? ToMultipartFormDataContent("png_sticker", PngSticker)
                : base.ToHttpContent();
    }
}
